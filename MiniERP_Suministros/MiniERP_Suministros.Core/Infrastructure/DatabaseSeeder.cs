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
                                      "admin@ebenmonney.com",
                                      "+1 (123) 000-0000",
                                      [adminRoleName]);

                await CreateUserAsync("user",
                                      "tempP@ss123",
                                      "Usuario Estándar Integrado",
                                      "user@ebenmonney.com",
                                      "+1 (123) 000-0001",
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
                    Id = 1,
                    Name = "Laptops",
                    Description = "Laptops y portátiles para oficina y hogar.",
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-10T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-10T08:00:00Z")
                };
                var catAccesorios = new ProductCategory
                {
                    Id = 2,
                    Name = "Accesorios",
                    Description = "Accesorios de computación y suministros.",
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-10T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-10T08:00:00Z")
                };
                var catImpresoras = new ProductCategory
                {
                    Id = 3,
                    Name = "Impresoras",
                    Description = "Impresoras y multifuncionales.",
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-12T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-12T08:00:00Z")
                };
                dbContext.ProductCategories.AddRange(catLaptops, catAccesorios, catImpresoras);

                // Productos
                var prod1 = new Product
                {
                    Id = 1,
                    Name = "Laptop Dell Inspiron 15",
                    Description = "Laptop para oficina, Intel i5, 8GB RAM, 256GB SSD.",
                    BuyingPrice = 500000.00m,
                    SellingPrice = 550000.00m,
                    UnitsInStock = 10,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategoryId = 1,
                    ProductCategory = catLaptops,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-10T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-10T08:00:00Z")
                };
                var prod2 = new Product
                {
                    Id = 2,
                    Name = "Mouse Logitech M185",
                    Description = "Mouse inalámbrico, color negro.",
                    BuyingPrice = 6000.00m,
                    SellingPrice = 8000.00m,
                    UnitsInStock = 50,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategoryId = 2,
                    ProductCategory = catAccesorios,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-10T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-10T08:00:00Z")
                };
                var prod3 = new Product
                {
                    Id = 3,
                    Name = "Teclado Microsoft Wired 600",
                    Description = "Teclado alámbrico, español latino.",
                    BuyingPrice = 7000.00m,
                    SellingPrice = 9500.00m,
                    UnitsInStock = 30,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategoryId = 2,
                    ProductCategory = catAccesorios,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-10T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-10T08:00:00Z")
                };
                var prod4 = new Product
                {
                    Id = 4,
                    Name = "Impresora HP DeskJet 2135",
                    Description = "Impresora multifuncional, color.",
                    BuyingPrice = 35000.00m,
                    SellingPrice = 42000.00m,
                    UnitsInStock = 15,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategoryId = 3,
                    ProductCategory = catImpresoras,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-12T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-12T08:00:00Z")
                };
                var prod5 = new Product
                {
                    Id = 5,
                    Name = "Cartucho HP 664XL Negro",
                    Description = "Cartucho de tinta original HP.",
                    BuyingPrice = 12000.00m,
                    SellingPrice = 15000.00m,
                    UnitsInStock = 40,
                    IsActive = true,
                    IsDiscontinued = false,
                    ProductCategoryId = 3,
                    ProductCategory = catImpresoras,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-12T08:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-12T08:00:00Z")
                };
                dbContext.Products.AddRange(prod1, prod2, prod3, prod4, prod5);

                // Clientes
                var cust1 = new Customer
                {
                    Id = 1,
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
                    Id = 2,
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
                    Id = 3,
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
                dbContext.Customers.AddRange(cust1, cust2, cust3);

                // Órdenes
                var order1 = new Order
                {
                    Id = 1,
                    Discount = 5000.00m,
                    Comments = "Venta mayorista.",
                    CashierId = "d4925699-1c2c-479e-b750-ec048c765afd",
                    CustomerId = 1,
                    Customer = cust1,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-12T10:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-12T10:00:00Z")
                };
                var order2 = new Order
                {
                    Id = 2,
                    Discount = 0.00m,
                    Comments = "Venta mostrador.",
                    CashierId = "b7de05eb-67fa-4ded-b8c2-355fc6602f12",
                    CustomerId = 2,
                    Customer = cust2,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-13T11:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-13T11:00:00Z")
                };
                var order3 = new Order
                {
                    Id = 3,
                    Discount = 1000.00m,
                    Comments = "Descuento especial por volumen.",
                    CashierId = "d4925699-1c2c-479e-b750-ec048c765afd",
                    CustomerId = 2,
                    Customer = cust2,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-14T12:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-14T12:00:00Z")
                };
                var order4 = new Order
                {
                    Id = 4,
                    Discount = 2000.00m,
                    Comments = "Compra de insumos para oficina.",
                    CashierId = "b7de05eb-67fa-4ded-b8c2-355fc6602f12",
                    CustomerId = 3,
                    Customer = cust3,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-15T13:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-15T13:00:00Z")
                };
                dbContext.Orders.AddRange(order1, order2, order3, order4);

                // Detalles de órdenes
                var od1 = new OrderDetail
                {
                    Id = 1,
                    UnitPrice = 550000.00m,
                    Quantity = 2,
                    Discount = 5000.00m,
                    ProductId = 1,
                    Product = prod1,
                    OrderId = 1,
                    Order = order1,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-12T10:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-12T10:00:00Z")
                };
                var od2 = new OrderDetail
                {
                    Id = 2,
                    UnitPrice = 8000.00m,
                    Quantity = 5,
                    Discount = 0.00m,
                    ProductId = 2,
                    Product = prod2,
                    OrderId = 1,
                    Order = order1,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-12T10:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-12T10:00:00Z")
                };
                var od3 = new OrderDetail
                {
                    Id = 3,
                    UnitPrice = 9500.00m,
                    Quantity = 1,
                    Discount = 0.00m,
                    ProductId = 3,
                    Product = prod3,
                    OrderId = 2,
                    Order = order2,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-13T11:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-13T11:00:00Z")
                };
                var od4 = new OrderDetail
                {
                    Id = 4,
                    UnitPrice = 8000.00m,
                    Quantity = 2,
                    Discount = 1000.00m,
                    ProductId = 2,
                    Product = prod2,
                    OrderId = 3,
                    Order = order3,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-14T12:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-14T12:00:00Z")
                };
                var od5 = new OrderDetail
                {
                    Id = 5,
                    UnitPrice = 42000.00m,
                    Quantity = 1,
                    Discount = 2000.00m,
                    ProductId = 4,
                    Product = prod4,
                    OrderId = 4,
                    Order = order4,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-15T13:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-15T13:00:00Z")
                };
                var od6 = new OrderDetail
                {
                    Id = 6,
                    UnitPrice = 15000.00m,
                    Quantity = 3,
                    Discount = 0.00m,
                    ProductId = 5,
                    Product = prod5,
                    OrderId = 4,
                    Order = order4,
                    CreatedBy = "SYSTEM",
                    UpdatedBy = "SYSTEM",
                    CreatedDate = DateTime.Parse("2024-01-15T13:00:00Z"),
                    UpdatedDate = DateTime.Parse("2024-01-15T13:00:00Z")
                };
                dbContext.OrderDetails.AddRange(od1, od2, od3, od4, od5, od6);

                await dbContext.SaveChangesAsync();

                logger.LogInformation("Datos de demostración sembrados correctamente");
            }
        }
    }
}
