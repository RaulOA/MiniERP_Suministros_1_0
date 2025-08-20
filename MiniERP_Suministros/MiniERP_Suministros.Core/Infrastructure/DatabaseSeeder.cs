using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiniERP_Suministros.Core.Models;
using MiniERP_Suministros.Core.Models.Account;
using MiniERP_Suministros.Core.Models.Shop;
using MiniERP_Suministros.Core.Services.Account;

namespace MiniERP_Suministros.Core.Infrastructure
{
    public class DatabaseSeeder(ApplicationDbContext dbContext, ILogger<DatabaseSeeder> logger,
        IUserAccountService userAccountService, IUserRoleService userRoleService) : IDatabaseSeeder
    {
        public async Task SeedAsync()
        {
            await dbContext.Database.MigrateAsync();
            await SeedDefaultUsersAsync();
            await SeedDemoDataAsync();
        }

        /************ USUARIOS POR DEFECTO **************/

        private async Task SeedDefaultUsersAsync()
        {
            if (!await dbContext.Users.AnyAsync())
            {
                logger.LogInformation("Generando cuentas integradas");

                const string adminRoleName = "administrator";
                const string userRoleName = "user";

                await EnsureRoleAsync(adminRoleName, "Administrador por defecto",
                    ApplicationPermissions.GetAllPermissionValues());

                await EnsureRoleAsync(userRoleName, "Usuario por defecto", []);

                await CreateUserAsync("admin",
                                      "tempP@ss123",
                                      "Administrador Integrado",
                                      "admin@prueba.com",
                                      "(506) 000-0000",
                                      [adminRoleName]);

                await CreateUserAsync("user",
                                      "tempP@ss123",
                                      "Usuario Estándar Integrado",
                                      "user@prueba.com",
                                      "(506) 000-0000",
                                      [userRoleName]);

                logger.LogInformation("Generación de cuentas integradas completada");
            }
        }

        private async Task EnsureRoleAsync(string roleName, string description, string[] claims)
        {
            if (await userRoleService.GetRoleByNameAsync(roleName) == null)
            {
                logger.LogInformation("Generando rol por defecto: {roleName}", roleName);

                var applicationRole = new ApplicationRole(roleName, description);

                var result = await userRoleService.CreateRoleAsync(applicationRole, claims);

                if (!result.Succeeded)
                {
                    throw new UserRoleException($"Error al sembrar el rol \"{description}\". Errores: " +
                        $"{string.Join(Environment.NewLine, result.Errors)}");
                }
            }
        }

        private async Task<ApplicationUser> CreateUserAsync(
            string userName, string password, string fullName, string email, string phoneNumber, string[] roles)
        {
            logger.LogInformation("Generando usuario por defecto: {userName}", userName);

            var applicationUser = new ApplicationUser
            {
                UserName = userName,
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                EmailConfirmed = true,
                IsEnabled = true
            };

            var result = await userAccountService.CreateUserAsync(applicationUser, roles, password);

            if (!result.Succeeded)
            {
                throw new UserAccountException($"Error al sembrar el usuario \"{userName}\". Errores: " +
                    $"{string.Join(Environment.NewLine, result.Errors)}");
            }

            return applicationUser;
        }

        /************ DATOS DE DEMOSTRACIÓN **************/

        private async Task SeedDemoDataAsync()
        {
            if (!await dbContext.Customers.AnyAsync() && !await dbContext.ProductCategories.AnyAsync())
            {
                logger.LogInformation("Sembrando datos de demostración");

                // Categorías de productos
                var catLaptops = new ProductCategory
                {
                    Name = "Laptops",
                    Description = "Laptops y portátiles para oficina y hogar.",
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-10T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-10T08:00:00Z")
                };
                var catAccesorios = new ProductCategory
                {
                    Name = "Accesorios",
                    Description = "Accesorios de computación y suministros.",
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-10T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-10T08:00:00Z")
                };
                var catImpresoras = new ProductCategory
                {
                    Name = "Impresoras",
                    Description = "Impresoras y multifuncionales.",
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-12T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-12T08:00:00Z")
                };

                // +10 categorías adicionales
                var catMonitores = new ProductCategory
                {
                    Name = "Monitores",
                    Description = "Monitores LED y IPS para trabajo y gaming.",
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-16T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-16T08:00:00Z")
                };
                var catRedes = new ProductCategory
                {
                    Name = "Redes",
                    Description = "Routers, switches y tarjetas de red.",
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-16T08:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-16T08:30:00Z")
                };
                var catAlmacenamiento = new ProductCategory
                {
                    Name = "Almacenamiento",
                    Description = "Discos duros, SSD y memorias externas.",
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-17T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-17T08:00:00Z")
                };
                var catSoftware = new ProductCategory
                {
                    Name = "Software",
                    Description = "Sistemas operativos, ofimática y seguridad.",
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-18T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-18T08:00:00Z")
                };
                var catComponentes = new ProductCategory
                {
                    Name = "Componentes",
                    Description = "Memorias, tarjetas madre y procesadores.",
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-19T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-19T08:00:00Z")
                };
                var catAudioVideo = new ProductCategory
                {
                    Name = "Audio y Video",
                    Description = "Audífonos, bocinas y webcams.",
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-20T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-20T08:00:00Z")
                };
                var catGamer = new ProductCategory
                {
                    Name = "Gamer",
                    Description = "Periféricos y sillas para gaming.",
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-21T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-21T08:00:00Z")
                };
                var catTablets = new ProductCategory
                {
                    Name = "Tablets",
                    Description = "Tablets para consumo y educación.",
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-22T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-22T08:00:00Z")
                };
                var catSmartphones = new ProductCategory
                {
                    Name = "Smartphones",
                    Description = "Teléfonos inteligentes y accesorios.",
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-23T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-23T08:00:00Z")
                };
                var catMobiliario = new ProductCategory
                {
                    Name = "Sillas y Escritorios",
                    Description = "Mobiliario ergonómico para oficina.",
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-24T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-24T08:00:00Z")
                };

                dbContext.ProductCategories.AddRange(
                    catLaptops, catAccesorios, catImpresoras,
                    catMonitores, catRedes, catAlmacenamiento, catSoftware, catComponentes,
                    catAudioVideo, catGamer, catTablets, catSmartphones, catMobiliario
                );

                // Productos
                var prod1 = new Product
                {
                    Name = "Laptop Dell Inspiron 15",
                    Description = "Laptop para oficina, Intel i5, 8GB RAM, 256GB SSD.",
                    BuyingPrice = 500000.00m,
                    SellingPrice = 550000.00m,
                    UnitsInStock = 10,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategory = catLaptops,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-10T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-10T08:00:00Z")
                };
                var prod2 = new Product
                {
                    Name = "Mouse Logitech M185",
                    Description = "Mouse inalámbrico, color negro.",
                    BuyingPrice = 6000.00m,
                    SellingPrice = 8000.00m,
                    UnitsInStock = 50,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategory = catAccesorios,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-10T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-10T08:00:00Z")
                };
                var prod3 = new Product
                {
                    Name = "Teclado Microsoft Wired 600",
                    Description = "Teclado alámbrico, español latino.",
                    BuyingPrice = 7000.00m,
                    SellingPrice = 9500.00m,
                    UnitsInStock = 30,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategory = catAccesorios,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-10T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-10T08:00:00Z")
                };
                var prod4 = new Product
                {
                    Name = "Impresora HP DeskJet 2135",
                    Description = "Impresora multifuncional, color.",
                    BuyingPrice = 35000.00m,
                    SellingPrice = 42000.00m,
                    UnitsInStock = 15,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategory = catImpresoras,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-12T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-12T08:00:00Z")
                };
                var prod5 = new Product
                {
                    Name = "Cartucho HP 664XL Negro",
                    Description = "Cartucho de tinta original HP.",
                    BuyingPrice = 12000.00m,
                    SellingPrice = 15000.00m,
                    UnitsInStock = 40,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategory = catImpresoras,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-12T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-12T08:00:00Z")
                };

                // +10 productos adicionales
                var prod6 = new Product
                {
                    Name = "Monitor Samsung 24\" IPS",
                    Description = "Monitor 24 pulgadas IPS, Full HD, 75Hz.",
                    BuyingPrice = 80000.00m,
                    SellingPrice = 95000.00m,
                    UnitsInStock = 25,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategory = catMonitores,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-16T09:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-16T09:00:00Z")
                };
                var prod7 = new Product
                {
                    Name = "Switch TP-Link 8 Puertos",
                    Description = "Switch no administrable 10/100/1000.",
                    BuyingPrice = 18000.00m,
                    SellingPrice = 23000.00m,
                    UnitsInStock = 35,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategory = catRedes,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-16T09:15:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-16T09:15:00Z")
                };
                var prod8 = new Product
                {
                    Name = "SSD Kingston 480GB",
                    Description = "Unidad de estado sólido SATA 2.5\".",
                    BuyingPrice = 25000.00m,
                    SellingPrice = 32000.00m,
                    UnitsInStock = 60,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategory = catAlmacenamiento,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-17T09:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-17T09:00:00Z")
                };
                var prod9 = new Product
                {
                    Name = "Licencia Office Hogar",
                    Description = "Suite de ofimática para 1 PC, perpetua.",
                    BuyingPrice = 65000.00m,
                    SellingPrice = 82000.00m,
                    UnitsInStock = 20,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategory = catSoftware,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-18T09:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-18T09:00:00Z")
                };
                var prod10 = new Product
                {
                    Name = "Memoria RAM 16GB DDR4 3200",
                    Description = "Módulo de memoria para desktop.",
                    BuyingPrice = 18000.00m,
                    SellingPrice = 24000.00m,
                    UnitsInStock = 45,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategory = catComponentes,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-19T09:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-19T09:00:00Z")
                };
                var prod11 = new Product
                {
                    Name = "Auriculares Logitech H390",
                    Description = "Diadema USB con micrófono con cancelación.",
                    BuyingPrice = 20000.00m,
                    SellingPrice = 26000.00m,
                    UnitsInStock = 28,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategory = catAudioVideo,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-20T09:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-20T09:00:00Z")
                };
                var prod12 = new Product
                {
                    Name = "Silla Gamer Cougar Armor",
                    Description = "Silla ergonómica con ajuste de altura.",
                    BuyingPrice = 110000.00m,
                    SellingPrice = 135000.00m,
                    UnitsInStock = 12,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategory = catGamer,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-21T09:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-21T09:00:00Z")
                };
                var prod13 = new Product
                {
                    Name = "Tablet Samsung Galaxy Tab A8",
                    Description = "Pantalla 10.5\", 64GB, WiFi.",
                    BuyingPrice = 120000.00m,
                    SellingPrice = 145000.00m,
                    UnitsInStock = 18,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategory = catTablets,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-22T09:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-22T09:00:00Z")
                };
                var prod14 = new Product
                {
                    Name = "Smartphone Xiaomi Redmi Note 12",
                    Description = "128GB, 6GB RAM, Dual SIM.",
                    BuyingPrice = 115000.00m,
                    SellingPrice = 139000.00m,
                    UnitsInStock = 22,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategory = catSmartphones,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-23T09:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-23T09:00:00Z")
                };
                var prod15 = new Product
                {
                    Name = "Router TP-Link AX1800 WiFi 6",
                    Description = "Router de doble banda con MU-MIMO.",
                    BuyingPrice = 52000.00m,
                    SellingPrice = 65000.00m,
                    UnitsInStock = 26,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategory = catRedes,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-24T09:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-24T09:00:00Z")
                };

                dbContext.Products.AddRange(
                    prod1, prod2, prod3, prod4, prod5,
                    prod6, prod7, prod8, prod9, prod10,
                    prod11, prod12, prod13, prod14, prod15
                );

                // Clientes
                var cust1 = new Customer
                {
                    Name = "Carlos Jiménez",
                    Email = "carlos.jimenez@suministroscr.com",
                    PhoneNumber = "+50688881234",
                    Address = "San José, Barrio Escalante",
                    City = "San José",
                    Gender = Gender.Male,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-10T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-10T08:00:00Z")
                };
                var cust2 = new Customer
                {
                    Name = "María Rodríguez",
                    Email = "maria.rodriguez@suministroscr.com",
                    PhoneNumber = "+50689991234",
                    Address = "Heredia, San Francisco",
                    City = "Heredia",
                    Gender = Gender.Female,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-11T09:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-11T09:00:00Z")
                };
                var cust3 = new Customer
                {
                    Name = "Luis Brenes",
                    Email = "luis.brenes@suministroscr.com",
                    PhoneNumber = "+50688776655",
                    Address = "Cartago, Centro",
                    City = "Cartago",
                    Gender = Gender.Male,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-12T10:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-12T10:00:00Z")
                };

                // +10 clientes adicionales
                var cust4 = new Customer
                {
                    Name = "Ana Solís",
                    Email = "ana.solis@suministroscr.com",
                    PhoneNumber = "+50688110022",
                    Address = "Alajuela, Centro",
                    City = "Alajuela",
                    Gender = Gender.Female,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-13T08:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-13T08:30:00Z")
                };
                var cust5 = new Customer
                {
                    Name = "Jorge Mora",
                    Email = "jorge.mora@suministroscr.com",
                    PhoneNumber = "+50688223344",
                    Address = "Puntarenas, Barranca",
                    City = "Puntarenas",
                    Gender = Gender.Male,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-13T09:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-13T09:00:00Z")
                };
                var cust6 = new Customer
                {
                    Name = "Valeria Sánchez",
                    Email = "valeria.sanchez@suministroscr.com",
                    PhoneNumber = "+50688335566",
                    Address = "Guanacaste, Liberia",
                    City = "Liberia",
                    Gender = Gender.Female,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-14T09:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-14T09:00:00Z")
                };
                var cust7 = new Customer
                {
                    Name = "Esteban Núñez",
                    Email = "esteban.nunez@suministroscr.com",
                    PhoneNumber = "+50688447788",
                    Address = "Limón, Centro",
                    City = "Limón",
                    Gender = Gender.Male,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-15T09:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-15T09:00:00Z")
                };
                var cust8 = new Customer
                {
                    Name = "Sofía Calderón",
                    Email = "sofia.calderon@suministroscr.com",
                    PhoneNumber = "+50688559911",
                    Address = "San José, Escazú",
                    City = "San José",
                    Gender = Gender.Female,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-16T09:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-16T09:00:00Z")
                };
                var cust9 = new Customer
                {
                    Name = "Ricardo Vargas",
                    Email = "ricardo.vargas@suministroscr.com",
                    PhoneNumber = "+50688667788",
                    Address = "Heredia, Belén",
                    City = "Heredia",
                    Gender = Gender.Male,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-17T09:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-17T09:00:00Z")
                };
                var cust10 = new Customer
                {
                    Name = "Daniela Pineda",
                    Email = "daniela.pineda@suministroscr.com",
                    PhoneNumber = "+50688779900",
                    Address = "Cartago, Tres Ríos",
                    City = "Cartago",
                    Gender = Gender.Female,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-18T09:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-18T09:00:00Z")
                };
                var cust11 = new Customer
                {
                    Name = "Andrés Quesada",
                    Email = "andres.quesada@suministroscr.com",
                    PhoneNumber = "+50688880011",
                    Address = "Alajuela, Grecia",
                    City = "Alajuela",
                    Gender = Gender.Male,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-19T09:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-19T09:00:00Z")
                };
                var cust12 = new Customer
                {
                    Name = "Natalia Rojas",
                    Email = "natalia.rojas@suministroscr.com",
                    PhoneNumber = "+50688992233",
                    Address = "San José, Santa Ana",
                    City = "San José",
                    Gender = Gender.Female,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-20T09:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-20T09:00:00Z")
                };
                var cust13 = new Customer
                {
                    Name = "Pablo Castillo",
                    Email = "pablo.castillo@suministroscr.com",
                    PhoneNumber = "+50688114455",
                    Address = "Heredia, Santo Domingo",
                    City = "Heredia",
                    Gender = Gender.Male,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-21T09:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-21T09:00:00Z")
                };

                dbContext.Customers.AddRange(
                    cust1, cust2, cust3,
                    cust4, cust5, cust6, cust7, cust8,
                    cust9, cust10, cust11, cust12, cust13
                );

                // Obtener el usuario admin y user para asignar como cajeros
                var adminUser = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == "admin");
                var normalUser = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == "user");

                // Órdenes
                var order1 = new Order
                {
                    Discount = 5000.00m,
                    Comments = "Venta mayorista.",
                    CashierId = adminUser?.Id, // admin
                    Customer = cust1,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-12T10:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-12T10:00:00Z")
                };
                var order2 = new Order
                {
                    Discount = 0.00m,
                    Comments = "Venta mostrador.",
                    CashierId = normalUser?.Id, // user
                    Customer = cust2,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-13T11:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-13T11:00:00Z")
                };
                var order3 = new Order
                {
                    Discount = 1000.00m,
                    Comments = "Descuento especial por volumen.",
                    CashierId = adminUser?.Id, // admin
                    Customer = cust2,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-14T12:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-14T12:00:00Z")
                };
                var order4 = new Order
                {
                    Discount = 2000.00m,
                    Comments = "Compra de insumos para oficina.",
                    CashierId = normalUser?.Id, // user
                    Customer = cust3,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-15T13:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-15T13:00:00Z")
                };

                // +10 órdenes adicionales
                var order5 = new Order
                {
                    Discount = 0.00m,
                    Comments = "Entrega inmediata.",
                    CashierId = adminUser?.Id,
                    Customer = cust4,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-16T10:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-16T10:30:00Z")
                };
                var order6 = new Order
                {
                    Discount = 1500.00m,
                    Comments = "Cliente frecuente.",
                    CashierId = normalUser?.Id,
                    Customer = cust5,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-17T11:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-17T11:30:00Z")
                };
                var order7 = new Order
                {
                    Discount = 0.00m,
                    Comments = "Venta web.",
                    CashierId = adminUser?.Id,
                    Customer = cust6,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-18T12:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-18T12:30:00Z")
                };
                var order8 = new Order
                {
                    Discount = 2500.00m,
                    Comments = "Combo promocional.",
                    CashierId = normalUser?.Id,
                    Customer = cust7,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-19T13:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-19T13:30:00Z")
                };
                var order9 = new Order
                {
                    Discount = 0.00m,
                    Comments = "Retiro en tienda.",
                    CashierId = adminUser?.Id,
                    Customer = cust8,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-20T14:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-20T14:30:00Z")
                };
                var order10 = new Order
                {
                    Discount = 3000.00m,
                    Comments = "Descuento por miembro corporativo.",
                    CashierId = normalUser?.Id,
                    Customer = cust9,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-21T15:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-21T15:30:00Z")
                };
                var order11 = new Order
                {
                    Discount = 0.00m,
                    Comments = "Envío a domicilio.",
                    CashierId = adminUser?.Id,
                    Customer = cust10,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-22T16:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-22T16:30:00Z")
                };
                var order12 = new Order
                {
                    Discount = 500.00m,
                    Comments = "Redondeo por promoción.",
                    CashierId = normalUser?.Id,
                    Customer = cust11,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-23T17:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-23T17:30:00Z")
                };
                var order13 = new Order
                {
                    Discount = 0.00m,
                    Comments = "Compra institucional.",
                    CashierId = adminUser?.Id,
                    Customer = cust12,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-24T18:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-24T18:30:00Z")
                };
                var order14 = new Order
                {
                    Discount = 1200.00m,
                    Comments = "Cliente nuevo, cortesía.",
                    CashierId = normalUser?.Id,
                    Customer = cust13,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-25T19:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-25T19:30:00Z")
                };

                dbContext.Orders.AddRange(
                    order1, order2, order3, order4,
                    order5, order6, order7, order8, order9,
                    order10, order11, order12, order13, order14
                );

                // Detalles de órdenes
                var od1 = new OrderDetail
                {
                    UnitPrice = 550000.00m,
                    Quantity = 2,
                    Discount = 5000.00m,
                    Product = prod1,
                    Order = order1,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-12T10:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-12T10:00:00Z")
                };
                var od2 = new OrderDetail
                {
                    UnitPrice = 8000.00m,
                    Quantity = 5,
                    Discount = 0.00m,
                    Product = prod2,
                    Order = order1,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-12T10:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-12T10:00:00Z")
                };
                var od3 = new OrderDetail
                {
                    UnitPrice = 9500.00m,
                    Quantity = 1,
                    Discount = 0.00m,
                    Product = prod3,
                    Order = order2,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-13T11:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-13T11:00:00Z")
                };
                var od4 = new OrderDetail
                {
                    UnitPrice = 8000.00m,
                    Quantity = 2,
                    Discount = 1000.00m,
                    Product = prod2,
                    Order = order3,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-14T12:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-14T12:00:00Z")
                };
                var od5 = new OrderDetail
                {
                    UnitPrice = 42000.00m,
                    Quantity = 1,
                    Discount = 2000.00m,
                    Product = prod4,
                    Order = order4,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-15T13:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-15T13:00:00Z")
                };
                var od6 = new OrderDetail
                {
                    UnitPrice = 15000.00m,
                    Quantity = 3,
                    Discount = 0.00m,
                    Product = prod5,
                    Order = order4,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-15T13:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-15T13:00:00Z")
                };

                // +10 detalles adicionales (uno por cada nueva orden)
                var od7 = new OrderDetail
                {
                    UnitPrice = 95000.00m,
                    Quantity = 1,
                    Discount = 0.00m,
                    Product = prod6,
                    Order = order5,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-16T10:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-16T10:30:00Z")
                };
                var od8 = new OrderDetail
                {
                    UnitPrice = 23000.00m,
                    Quantity = 2,
                    Discount = 1500.00m,
                    Product = prod7,
                    Order = order6,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-17T11:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-17T11:30:00Z")
                };
                var od9 = new OrderDetail
                {
                    UnitPrice = 32000.00m,
                    Quantity = 2,
                    Discount = 0.00m,
                    Product = prod8,
                    Order = order7,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-18T12:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-18T12:30:00Z")
                };
                var od10 = new OrderDetail
                {
                    UnitPrice = 82000.00m,
                    Quantity = 1,
                    Discount = 2500.00m,
                    Product = prod9,
                    Order = order8,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-19T13:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-19T13:30:00Z")
                };
                var od11 = new OrderDetail
                {
                    UnitPrice = 24000.00m,
                    Quantity = 2,
                    Discount = 0.00m,
                    Product = prod10,
                    Order = order9,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-20T14:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-20T14:30:00Z")
                };
                var od12 = new OrderDetail
                {
                    UnitPrice = 26000.00m,
                    Quantity = 1,
                    Discount = 3000.00m,
                    Product = prod11,
                    Order = order10,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-21T15:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-21T15:30:00Z")
                };
                var od13 = new OrderDetail
                {
                    UnitPrice = 135000.00m,
                    Quantity = 1,
                    Discount = 0.00m,
                    Product = prod12,
                    Order = order11,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-22T16:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-22T16:30:00Z")
                };
                var od14 = new OrderDetail
                {
                    UnitPrice = 145000.00m,
                    Quantity = 1,
                    Discount = 500.00m,
                    Product = prod13,
                    Order = order12,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-23T17:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-23T17:30:00Z")
                };
                var od15 = new OrderDetail
                {
                    UnitPrice = 139000.00m,
                    Quantity = 2,
                    Discount = 0.00m,
                    Product = prod14,
                    Order = order13,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-24T18:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-24T18:30:00Z")
                };
                var od16 = new OrderDetail
                {
                    UnitPrice = 65000.00m,
                    Quantity = 1,
                    Discount = 1200.00m,
                    Product = prod15,
                    Order = order14,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-25T19:30:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-25T19:30:00Z")
                };

                dbContext.OrderDetails.AddRange(
                    od1, od2, od3, od4, od5, od6,
                    od7, od8, od9, od10, od11, od12, od13, od14, od15, od16
                );

                await dbContext.SaveChangesAsync();

                logger.LogInformation("Datos de demostración sembrados correctamente");
            }
        }
    }
}
