using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using MiniERP_Suministros.Core.Infrastructure;
using MiniERP_Suministros.Core.Models.Account;
using MiniERP_Suministros.Core.Services;
using MiniERP_Suministros.Core.Services.Account;
using MiniERP_Suministros.Core.Services.Shop;
using MiniERP_Suministros.Server.Authorization;
using MiniERP_Suministros.Server.Authorization.Requirements;
using MiniERP_Suministros.Server.Configuration;
using MiniERP_Suministros.Server.Services;
using MiniERP_Suministros.Server.Services.Email;
using OpenIddict.Validation.AspNetCore;
using Quartz;
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);

/************* AGREGAR SERVICIOS *************/

/// <summary>
/// Obtiene la cadena de conexi�n llamada "DefaultConnection" de la configuraci�n de la aplicaci�n.
/// Lanza una <see cref="InvalidOperationException"/> si no se encuentra la cadena de conexi�n.
/// </summary>
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("No se encontr� la cadena de conexi�n 'DefaultConnection'.");

var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

// Configuraci�n del contexto de base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssembly));
    options.UseOpenIddict();
});

// Agregar Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configuraci�n de opciones de Identity y complejidad de contrase�as
builder.Services.Configure<IdentityOptions>(options =>
{
    // Configuraci�n de usuario
    options.User.RequireUniqueEmail = true;

    // Configuraci�n de contrase�a
    /*
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;

    // Configuraci�n de bloqueo
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 10;
    */

    // Configura Identity para usar los mismos claims JWT que OpenIddict
    options.ClaimsIdentity.UserNameClaimType = Claims.Name;
    options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
    options.ClaimsIdentity.RoleClaimType = Claims.Role;
    options.ClaimsIdentity.EmailClaimType = Claims.Email;
});

// Configuraci�n de limpieza peri�dica de autorizaciones/tokens hu�rfanos en la base de datos con OpenIddict
builder.Services.AddQuartz(options =>
{
    options.UseSimpleTypeLoader();
    options.UseInMemoryStore();
});

// Registrar el servicio de Quartz.NET y configurarlo para bloquear el apagado hasta que los trabajos finalicen
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

// Configuraci�n de OpenIddict
builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
               .UseDbContext<ApplicationDbContext>();

        options.UseQuartz();
    })
    .AddServer(options =>
    {
        options.SetTokenEndpointUris("connect/token");

        options.AllowPasswordFlow()
               .AllowRefreshTokenFlow();

        options.RegisterScopes(
            Scopes.Profile,
            Scopes.Email,
            Scopes.Address,
            Scopes.Phone,
            Scopes.Roles);

        if (builder.Environment.IsDevelopment())
        {
            options.AddDevelopmentEncryptionCertificate()
                   .AddDevelopmentSigningCertificate();
        }
        else
        {
            var oidcCertFileName = builder.Configuration["OIDC:Certificates:Path"];
            var oidcCertFilePassword = builder.Configuration["OIDC:Certificates:Password"];

            if (string.IsNullOrWhiteSpace(oidcCertFileName))
            {
                // Debe configurar claves persistentes para Encriptaci�n y Firma.
                // Ver https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html
                options.AddEphemeralEncryptionKey()
                       .AddEphemeralSigningKey();
            }
            else
            {
                var oidcCertificate = X509CertificateLoader.LoadPkcs12FromFile(oidcCertFileName, oidcCertFilePassword);

                options.AddEncryptionCertificate(oidcCertificate)
                       .AddSigningCertificate(oidcCertificate);
            }
        }

        options.UseAspNetCore()
               .EnableTokenEndpointPassthrough();
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

// Configuraci�n de autenticaci�n
builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    o.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});

// Configuraci�n de autorizaci�n y pol�ticas personalizadas
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AuthPolicies.ViewAllUsersPolicy,
        policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.ViewUsers))
    .AddPolicy(AuthPolicies.ManageAllUsersPolicy,
        policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.ManageUsers))
    .AddPolicy(AuthPolicies.ViewAllRolesPolicy,
        policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.ViewRoles))
    .AddPolicy(AuthPolicies.ViewRoleByRoleNamePolicy,
        policy => policy.Requirements.Add(new ViewRoleAuthorizationRequirement()))
    .AddPolicy(AuthPolicies.ManageAllRolesPolicy,
        policy => policy.RequireClaim(CustomClaims.Permission, ApplicationPermissions.ManageRoles))
    .AddPolicy(AuthPolicies.AssignAllowedRolesPolicy,
        policy => policy.Requirements.Add(new AssignRolesAuthorizationRequirement()));

// Agregar CORS
builder.Services.AddCors();

builder.Services.AddControllers();

// Configuraci�n de Swagger/OpenAPI
// M�s informaci�n: https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = OidcServerConfig.ServerName, Version = "v1" });
    c.OperationFilter<SwaggerAuthorizeOperationFilter>();
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Password = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri("/connect/token", UriKind.Relative)
            }
        }
    });
});

// Agregar AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Configuraciones generales
builder.Services.Configure<AppSettings>(builder.Configuration);

// Servicios de negocio
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>(); // Registro de categor�as de producto

// Otros servicios
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUserIdAccessor, UserIdAccessor>();

// Manejadores de autorizaci�n
builder.Services.AddSingleton<IAuthorizationHandler, ViewUserAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ManageUserAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ViewRoleAuthorizationHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, AssignRolesAuthorizationHandler>();

// Creaci�n y siembra de la base de datos
builder.Services.AddTransient<IDatabaseSeeder, DatabaseSeeder>();

// Logger de archivos
builder.Logging.AddFile(builder.Configuration.GetSection("Logging"));

// Plantillas de correo electr�nico
EmailTemplates.Initialize(builder.Environment);

var app = builder.Build();

/************* CONFIGURAR PIPELINE DE SOLICITUDES *************/

app.UseDefaultFiles();
app.MapStaticAssets();

if (app.Environment.IsDevelopment())
{
    // Configuraci�n de Swagger para desarrollo
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "Swagger UI - MiniERP_Suministros";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{OidcServerConfig.ServerName} V1");
        c.OAuthClientId(OidcServerConfig.SwaggerClientID);
    });

    IdentityModelEventSource.ShowPII = true;
}
else
{
    // El valor predeterminado de HSTS es 30 d�as.
    // Puede cambiar esto para escenarios de producci�n, ver https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Configuraci�n de CORS para permitir cualquier origen, encabezado y m�todo
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

/************* SIEMBRA DE BASE DE DATOS *************/

using var scope = app.Services.CreateScope();
try
{
    // Ejecuta la siembra de la base de datos y el registro de aplicaciones cliente OIDC
    var dbSeeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
    await dbSeeder.SeedAsync();

    await OidcServerConfig.RegisterClientApplicationsAsync(scope.ServiceProvider);
}
catch (Exception ex)
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogCritical(ex, "Ocurri� un error al crear/sembrar la base de datos");

    throw;
}

/************* EJECUTAR APLICACI�N *************/

app.Run();

