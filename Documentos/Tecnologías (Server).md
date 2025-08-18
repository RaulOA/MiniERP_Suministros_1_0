# Tecnologías, herramientas y dependencias del servidor (Claudes_2._0.Server)

Este documento describe en un solo apartado cada tecnología, herramienta, librería y dependencia utilizada en el backend del proyecto, incluyendo su propósito, ejemplos reales de uso, contexto de manipulación por el programador, y los archivos/rutas relevantes en la solución.

---

## 1. AutoMapper
- **¿Para qué sirve?**
  Convierte automáticamente objetos de un tipo a otro (por ejemplo, de entidad a DTO/ViewModel y viceversa).
- **Ejemplo real:**
  Cuando recibes datos de la base de datos (entidad) y necesitas enviarlos al frontend como un modelo más simple (ViewModel):
  ```csharp
  // En un controlador
  var userVM = _mapper.Map<UserVM>(userEntity);
  ```
- **¿Cuándo lo manipula el programador?**
  - Al crear nuevos modelos o DTOs.
  - Al modificar la estructura de datos que viaja entre backend y frontend.
  - Al agregar nuevos mapeos en `MappingProfile.cs`.
- **Archivos y rutas relevantes:**
  - Configuración de mapeos: `Claudes_2._0.Server/Configuration/MappingProfile.cs`
  - Registro en servicios: `Claudes_2._0.Server/Program.cs` (`builder.Services.AddAutoMapper(typeof(Program));`)
  - Uso en controladores: cualquier controlador que inyecte `IMapper` (ej: `UserRoleController.cs`, `CustomerController.cs`)

---

## 2. FluentValidation
- **¿Para qué sirve?**
  Permite validar datos de entrada de manera clara y reutilizable, separando la lógica de validación del modelo.
- **Ejemplo real:**
  Validar que un email sea válido antes de guardar un usuario:
  ```csharp
  public class UserVMValidator : AbstractValidator<UserVM>
  {
      public UserVMValidator()
      {
          RuleFor(x => x.Email).NotEmpty().EmailAddress();
      }
  }
  ```
- **¿Cuándo lo manipula el programador?**
  - Al definir reglas de validación para formularios o modelos.
  - Al agregar nuevas validaciones para datos de entrada.
- **Archivos y rutas relevantes:**
  - Registro en servicios: `Claudes_2._0.Server/Program.cs`
  - Validadores personalizados: por convención en `Claudes_2._0.Server/Validation/` o junto a los ViewModels
  - Uso en controladores: controladores con `[ApiController]` y validaciones automáticas

---

## 3. MailKit
- **¿Para qué sirve?**
  Enviar correos electrónicos desde la aplicación (notificaciones, confirmaciones, recuperación de contraseña).
- **Ejemplo real:**
  Enviar un correo de bienvenida al crear un usuario:
  ```csharp
  await _emailSender.SendEmailAsync("Nombre", "correo@dominio.com", "Asunto", "Mensaje");
  ```
- **¿Cuándo lo manipula el programador?**
  - Al implementar funcionalidades que requieran enviar emails.
  - Al personalizar plantillas de correo.
- **Archivos y rutas relevantes:**
  - Implementación: `Claudes_2._0.Server/Services/Email/EmailSender.cs`
  - Registro: `Claudes_2._0.Server/Program.cs` (`builder.Services.AddScoped<IEmailSender, EmailSender>();`)
  - Plantillas: `Claudes_2._0.Server/Services/Email/EmailTemplates.cs`, `Claudes_2._0.Server/Services/Email/*.template`

---

## 4. OpenIddict (OIDC/OAuth2)
- **¿Para qué sirve?**
  Gestiona la autenticación y autorización segura de usuarios usando estándares modernos (OpenID Connect/OAuth2).
- **Ejemplo real:**
  Permitir que los usuarios inicien sesión y obtengan un token de acceso:
  ```csharp
  // Configuración en OidcServerConfig.cs y Program.cs
  builder.Services.AddOpenIddict()...
  ```
- **¿Cuándo lo manipula el programador?**
  - Al configurar el login, permisos y roles.
  - Al agregar nuevos clientes (por ejemplo, una nueva SPA o app móvil).
- **Archivos y rutas relevantes:**
  - Configuración y registro de clientes: `Claudes_2._0.Server/Configuration/OidcServerConfig.cs`
  - Servicios: `Claudes_2._0.Server/Program.cs` (sección `builder.Services.AddOpenIddict()`)
  - Tablas y migraciones: `Claudes_2._0.Server/Migrations/20241114121916_Initial.cs`
  - Controladores: `Claudes_2._0.Server/Controllers/AuthorizationController.cs`

---

## 5. Quartz
- **¿Para qué sirve?**
  Permite programar tareas automáticas (jobs) que se ejecutan en segundo plano, como enviar reportes diarios o limpiar datos antiguos.
- **Ejemplo real:**
  Programar el envío de un resumen diario por correo:
  ```csharp
  builder.Services.AddQuartz(options => { ... });
  ```
- **¿Cuándo lo manipula el programador?**
  - Al crear o modificar tareas automáticas.
  - Al ajustar la frecuencia de ejecución de jobs.
- **Archivos y rutas relevantes:**
  - Configuración: `Claudes_2._0.Server/Program.cs` (sección `builder.Services.AddQuartz(...)`)
  - Jobs personalizados: por convención en `Claudes_2._0.Server/Services/Jobs/` (si existen)

---

## 6. Serilog
- **¿Para qué sirve?**
  Registrar logs (eventos, errores, advertencias) en archivos para auditoría y diagnóstico.
- **Ejemplo real:**
  Guardar un error cuando falla el acceso a la base de datos:
  ```csharp
  _logger.LogError(ex, "Error accediendo a la base de datos");
  ```
- **¿Cuándo lo manipula el programador?**
  - Al agregar nuevos mensajes de log en el código.
  - Al configurar el formato o destino de los logs.
- **Archivos y rutas relevantes:**
  - Configuración: `Claudes_2._0.Server/Program.cs` (`builder.Logging.AddFile(builder.Configuration.GetSection("Logging"));`)
  - Configuración adicional: puede estar en `appsettings.json`

---

## 7. Swashbuckle (Swagger/OpenAPI)
- **¿Para qué sirve?**
  Genera documentación interactiva de la API, permitiendo probar los endpoints desde el navegador.
- **Ejemplo real:**
  Ver y probar los endpoints en `/swagger`:
  ```csharp
  builder.Services.AddSwaggerGen(...);
  ```
- **¿Cuándo lo manipula el programador?**
  - Al documentar nuevos endpoints.
  - Al personalizar la documentación de la API.
- **Archivos y rutas relevantes:**
  - Configuración: `Claudes_2._0.Server/Program.cs` (sección `builder.Services.AddSwaggerGen(...)`)
  - Filtros y seguridad: `Claudes_2._0.Server/Authorization/SwaggerAuthorizeOperationFilter.cs`

---

## 8. Microsoft.AspNetCore.JsonPatch
- **¿Para qué sirve?**
  Permite actualizar parcialmente recursos usando el método HTTP PATCH (útil para editar solo algunos campos).
- **Ejemplo real:**
  Actualizar solo el email de un usuario sin enviar todo el objeto:
  ```csharp
  public async Task<IActionResult> PatchUser([FromBody] JsonPatchDocument<UserVM> patch)
  ```
- **¿Cuándo lo manipula el programador?**
  - Al implementar endpoints PATCH.
  - Al definir qué campos pueden ser modificados parcialmente.
- **Archivos y rutas relevantes:**
  - Uso en controladores: `Claudes_2._0.Server/Controllers/UserAccountController.cs` (y otros controladores que acepten `JsonPatchDocument`)

---

## 9. Entity Framework Core (EF Core)
- **¿Para qué sirve?**
  ORM para interactuar con la base de datos usando clases C# en vez de SQL directo.
- **Ejemplo real:**
  Obtener todos los clientes de la base de datos:
  ```csharp
  var clientes = await _context.Customers.ToListAsync();
  ```
- **¿Cuándo lo manipula el programador?**
  - Al crear, leer, actualizar o eliminar datos.
  - Al definir nuevas entidades o relaciones.
  - Al ejecutar migraciones para actualizar el esquema de la base de datos.
- **Archivos y rutas relevantes:**
  - Contexto: `Claudes_2._0.Core/Infrastructure/ApplicationDbContext.cs`
  - Migraciones: `Claudes_2._0.Server/Migrations/*.cs`
  - Configuración: `Claudes_2._0.Server/Program.cs` (sección `builder.Services.AddDbContext<ApplicationDbContext>(...)`)

---

## 10. Microsoft.AspNetCore.SpaProxy
- **¿Para qué sirve?**
  Permite que el backend y el frontend (Angular) trabajen juntos durante el desarrollo, redirigiendo peticiones automáticamente.
- **Ejemplo real:**
  Cuando ejecutas ambos proyectos, el backend redirige las rutas de Angular automáticamente.
- **¿Cuándo lo manipula el programador?**
  - Generalmente solo al configurar el proyecto por primera vez o al cambiar la integración entre backend y frontend.
- **Archivos y rutas relevantes:**
  - Configuración SPA: `Claudes_2._0.Server/Claudes_2._0.Server.csproj` (propiedades `<SpaRoot>`, `<SpaProxyLaunchCommand>`, `<SpaProxyServerUrl>`)
  - Lógica de integración: `Claudes_2._0.Server/Program.cs`

---

**Stack principal:**
- .NET 9.0, C# 13.0, ASP.NET Core 9, Entity Framework Core 9, AutoMapper, FluentValidation, OpenIddict, Quartz, Serilog, MailKit, Swashbuckle/Swagger, Microsoft.AspNetCore.JsonPatch, Microsoft.AspNetCore.SpaProxy, Angular 19 (SPA).

Este stack permite autenticación, autorización, validación, mapeo de datos, documentación, logging, envío de correos, tareas programadas y desarrollo integrado con SPA.

---
Por supuesto, aquí tienes la estructura existente del proyecto `Claudes_2._0.Server` con la nota solicitada:

```
Claudes_2._0.Server/
│
├── Claudes_2._0.Server.csproj                  # Archivo de proyecto del backend ASP.NET Core, referencia librerías y configura compilación
├── Program.cs                                  # Punto de entrada principal para construir y ejecutar el servidor (API, servicios, configuración)
├── appsettings.json                            # Configuración general de la aplicación: cadenas de conexión, parámetros globales
├── appsettings.Development.json                # Configuración específica para entorno de desarrollo (sobrepone valores de appsettings.json)
├── Properties/
│   └── launchSettings.json                     # Define perfiles de ejecución para depuración local (puerto, rutas, variables de entorno)
├── Configuration/
│   ├── MappingProfile.cs                       # Configuración de AutoMapper: define reglas de mapeo entre entidades y DTOs
│   └── OidcServerConfig.cs                     # Registro de clientes OIDC y configuración de permisos, scopes y flujos en OpenIddict
├── Controllers/
│   ├── AuthorizationController.cs              # Controlador que maneja el endpoint /connect/token y el flujo de autenticación OIDC
│   ├── UserAccountController.cs                # Endpoints para operaciones sobre cuentas de usuario (registro, actualización, recuperación)
│   ├── UserRoleController.cs                   # Endpoints para gestión de roles de usuario (asignación, consulta, modificación)
│   ├── CustomerController.cs                   # API REST para operaciones sobre clientes (Shop/Customer)
│   ├── ProductController.cs                    # API REST para consulta, creación y edición de productos
│   ├── OrdersController.cs                     # API REST para gestión de pedidos y detalles de órdenes
├── Services/
│   ├── Email/
│   │   ├── EmailSender.cs                      # Servicio para enviar correos electrónicos desde el servidor
│   │   ├── EmailTemplates.cs                   # Contiene plantillas HTML disponibles para envío por correo
│   │   ├── PlainTextTestEmail.template         # Plantilla de correo en formato texto plano para pruebas
│   │   └── TestEmail.template                  # Plantilla HTML de correo para pruebas de formato enriquecido
│   ├── Jobs/                                   # Carpeta para tareas programadas o background workers (actualmente vacía)
│   ├── Utilities.cs                            # Funciones auxiliares y utilidades compartidas para operaciones comunes
│   └── UserIdAccessor.cs                       # Servicio para obtener el ID del usuario autenticado en contexto actual
├── Authorization/
│   ├── SwaggerAuthorizeOperationFilter.cs      # Filtro para proteger operaciones Swagger según permisos de usuario
│   ├── ViewUserAuthorizationHandler.cs         # Handler que autoriza operaciones de vista sobre usuarios
│   ├── ManageUserAuthorizationHandler.cs       # Handler para autorizar operaciones de gestión sobre cuentas de usuario
│   ├── ViewRoleAuthorizationHandler.cs         # Handler para verificar permisos para visualizar roles disponibles
│   ├── AssignRolesAuthorizationHandler.cs      # Handler que controla quién puede asignar roles a otros usuarios
├── Migrations/
│   ├── 20241114121916_Initial.cs               # Primera migración de base de datos generada por EF Core
│   ├── 20241114121916_Initial.Designer.cs      # Archivo auxiliar generado automáticamente para describir la estructura de migración
```

**Notas:**
- La carpeta `Controllers/` contiene los controladores de la API.
- La carpeta `Services/` agrupa servicios de infraestructura, email, utilidades y jobs.
- La carpeta `Configuration/` contiene archivos de configuración avanzada (mapeos, OIDC, etc.).
- La carpeta `Authorization/` contiene handlers y filtros de autorización.
- La carpeta `Validation/` es para validadores de modelos (FluentValidation).
- La carpeta `Migrations/` contiene las migraciones de Entity Framework Core.
- La carpeta `wwwroot/` sirve archivos estáticos y la SPA Angular.
- La carpeta `Documentos/` contiene documentación técnica relevante.