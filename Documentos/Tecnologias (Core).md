# Tecnologías, herramientas y dependencias del core (Claudes_2._0.Core)

Este documento describe en un solo apartado cada tecnología, herramienta, librería y dependencia utilizada en el core del proyecto, incluyendo su propósito, ejemplos reales de uso, contexto de manipulación por el programador, y los archivos/rutas relevantes en la solución.

---

## 1. Entity Framework Core (EF Core)
- **¿Para qué sirve?**
  Permite mapear clases C# a tablas de base de datos y realizar operaciones CRUD sin escribir SQL manualmente.
- **Ejemplo real:**
  Definir una entidad y consultarla:
  ```csharp
  // Models/Shop/Product.cs
  public class Product {
      public int Id { get; set; }
      public string Name { get; set; }
  }
  // Uso en ApplicationDbContext.cs
  public DbSet<Product> Products { get; set; }
  // Consultar productos
  var productos = await _context.Products.ToListAsync();
  ```
- **¿Cuándo lo manipula el programador?**
  - Al crear, modificar o eliminar entidades del dominio.
  - Al definir relaciones entre entidades.
  - Al actualizar el esquema de la base de datos mediante migraciones.
- **Archivos y rutas relevantes:**
  - Entidades: `Claudes_2._0.Core/Models/` (por ejemplo, `Account/`, `Shop/`)
  - Contexto: `Claudes_2._0.Core/Infrastructure/ApplicationDbContext.cs`

---

## 2. ASP.NET Core Identity (Microsoft.AspNetCore.Identity.EntityFrameworkCore)
- **¿Para qué sirve?**
  Permite gestionar usuarios, roles y autenticación de manera segura y extensible.
- **Ejemplo real:**
  Definir un usuario personalizado:
  ```csharp
  // Models/Account/ApplicationUser.cs
  public class ApplicationUser : IdentityUser {
      public string FullName { get; set; }
  }
  ```
- **¿Cuándo lo manipula el programador?**
  - Al extender la información de usuario o rol.
  - Al implementar reglas de autenticación y autorización.
- **Archivos y rutas relevantes:**
  - Usuarios y roles: `Claudes_2._0.Core/Models/Account/ApplicationUser.cs`, `ApplicationRole.cs`
  - Permisos y claims: `Claudes_2._0.Core/Services/Account/`

---

## 3. Clases de Dominio y Servicios
- **¿Para qué sirve?**
  Organizan la lógica de negocio y las reglas del dominio de la aplicación.
- **Ejemplo real:**
  Definir un servicio para gestionar clientes:
  ```csharp
  // Services/Shop/CustomerService.cs
  public class CustomerService : ICustomerService {
      public Task<IEnumerable<Customer>> GetAllAsync() { ... }
  }
  ```
- **¿Cuándo lo manipula el programador?**
  - Al implementar reglas de negocio.
  - Al crear interfaces y servicios reutilizables.
- **Archivos y rutas relevantes:**
  - Servicios: `Claudes_2._0.Core/Services/` (por ejemplo, `Account/`, `Shop/`)
  - Interfaces: `Claudes_2._0.Core/Services/` (ej: `ICustomerService.cs`)

---

## 4. Modelos Base y Utilidades
- **¿Para qué sirve?**
  Proveen clases base y utilidades comunes para todo el dominio (por ejemplo, `BaseEntity`, helpers, enums).
- **Ejemplo real:**
  ```csharp
  // Models/BaseEntity.cs
  public abstract class BaseEntity {
      public int Id { get; set; }
      public DateTime CreatedDate { get; set; }
  }
  ```
- **¿Cuándo lo manipula el programador?**
  - Al definir propiedades comunes a todas las entidades.
  - Al crear utilidades compartidas.
- **Archivos y rutas relevantes:**
  - Modelos base: `Claudes_2._0.Core/Models/BaseEntity.cs`, `IAuditableEntity.cs`
  - Utilidades: `Claudes_2._0.Core/Infrastructure/`, `Claudes_2._0.Core/Helpers/`

---

## 5. Enumeraciones y Constantes
- **¿Para qué sirve?**
  Definen valores fijos y constantes usados en todo el dominio (por ejemplo, roles, permisos, estados).
- **Ejemplo real:**
  ```csharp
  // Models/Account/UserRoles.cs
  public static class UserRoles {
      public const string Admin = "Admin";
      public const string User = "User";
  }
  ```
- **¿Cuándo lo manipula el programador?**
  - Al agregar nuevos roles, permisos o estados.
- **Archivos y rutas relevantes:**
  - Enumeraciones: `Claudes_2._0.Core/Models/Account/`, `Claudes_2._0.Core/Models/Shop/`, `Claudes_2._0.Core/Models/Gender.cs`
  - Permisos: `Claudes_2._0.Core/Services/Account/ApplicationPermissions.cs`, `CustomClaims.cs`

---

## 6. Interfaces y Abstracciones
- **¿Para qué sirve?**
  Permiten definir contratos para servicios y repositorios, facilitando la inyección de dependencias y pruebas.
- **Ejemplo real:**
  ```csharp
  // Services/Shop/Interfaces/ICustomerService.cs
  public interface ICustomerService {
      Task<IEnumerable<Customer>> GetAllAsync();
  }
  ```
- **¿Cuándo lo manipula el programador?**
  - Al definir nuevos servicios o repositorios.
  - Al facilitar pruebas unitarias y mocks.
- **Archivos y rutas relevantes:**
  - Interfaces: `Claudes_2._0.Core/Services/Shop/Interfaces/`, `Claudes_2._0.Core/Services/Account/Interfaces/`

---

**Stack principal del core:**
- .NET 9.0, C# 13.0, Entity Framework Core 9, ASP.NET Core Identity, arquitectura por capas (entidades, servicios, interfaces, utilidades).

Este stack permite modelar el dominio, gestionar la lógica de negocio, acceder a la base de datos y mantener una arquitectura limpia y escalable.

---

Aquí tienes la estructura **existente** y completa del proyecto `Claudes_2._0.Core` según los archivos y carpetas detectados en tu solución:

```
Claudes_2._0.Core/
│
├── Claudes_2._0.Core.csproj                  # Archivo de proyecto principal .NET Core
├── Infrastructure/
│   ├── ApplicationDbContext.cs               # Contexto principal de EF Core para acceso y persistencia de datos
│   ├── DatabaseSeeder.cs                     # Clase de inicialización con datos de prueba en la base de datos
│   └── IDatabaseSeeder.cs                    # Interfaz para definir el contrato de siembra de datos
├── Models/
│   ├── Account/
│   │   ├── ApplicationPermission.cs          # Permisos personalizados utilizados en el sistema
│   │   ├── ApplicationRole.cs                # Representación de roles para Identity
│   │   ├── ApplicationUser.cs                # Modelo extendido de usuario para autenticación
│   │   └── UserRoles.cs                      # Constantes para roles y jerarquías de usuario
│   ├── BaseEntity.cs                         # Clase base para entidades con propiedades comunes (Id, Timestamps)
│   ├── Gender.cs                             # Enumeración que define tipos de género
│   ├── IAuditableEntity.cs                   # Interfaz para entidades que registran auditoría (creación/modificación)
│   └── Shop/
│       ├── Customer.cs                       # Modelo de entidad cliente
│       ├── Order.cs                          # Modelo de entidad pedido
│       ├── OrderDetail.cs                    # Detalle de los productos incluidos en un pedido
│       ├── Product.cs                        # Modelo de entidad producto
│       └── ProductCategory.cs                # Categoría asociada a un conjunto de productos
├── Services/
│   ├── Account/
│   │   ├── ApplicationPermissions.cs         # Implementación y agrupación de permisos definidos en el sistema
│   │   ├── CustomClaims.cs                   # Claims personalizados para extender la autenticación
│   │   ├── Exceptions/
│   │   │   ├── UserAccountException.cs       # Excepción para errores relacionados a cuentas de usuario
│   │   │   ├── UserNotFoundException.cs      # Excepción lanzada cuando un usuario no existe
│   │   │   └── UserRoleException.cs          # Excepción para conflictos o errores con roles
│   │   ├── Interfaces/
│   │   │   ├── IUserAccountService.cs        # Contrato para operaciones sobre cuentas de usuario
│   │   │   ├── IUserIdAccessor.cs            # Interfaz para obtener el ID del usuario autenticado
│   │   │   └── IUserRoleService.cs           # Contrato para operaciones sobre asignación de roles
│   │   ├── UserAccountService.cs             # Implementación del servicio de gestión de cuentas
│   │   └── UserRoleService.cs                # Servicio para la gestión y asignación de roles
│   ├── IEmailSender.cs                       # Interfaz para la funcionalidad de envío de correos electrónicos
│   ├── Shop/
│   │   ├── CustomerService.cs                # Lógica de negocio para manejar clientes
│   │   ├── Exceptions/
│   │   │   └── CustomerException.cs          # Excepción personalizada para errores relacionados a clientes
│   │   ├── Interfaces/
│   │   │   ├── ICustomerService.cs           # Contrato para el servicio de clientes
│   │   │   ├── IOrdersService.cs             # Contrato para el servicio de pedidos
│   │   │   └── IProductService.cs            # Contrato para el servicio de productos
│   │   ├── OrdersService.cs                  # Implementación del servicio para pedidos
│   │   └── ProductService.cs                 # Implementación del servicio para productos
├── Extensions/
│   ├── ArrayExtensions.cs                    # Métodos de extensión útiles para arreglos
│   └── StringExtensions.cs                   # Métodos de extensión para manipulación de cadenas
```

**Notas:**
- La carpeta `Models/` contiene las entidades del dominio, organizadas por contexto (Account, Shop, etc.).
- La carpeta `Services/` agrupa servicios de dominio, interfaces y excepciones.
- La carpeta `Infrastructure/` contiene el contexto de base de datos y utilidades de infraestructura.
- La carpeta `Extensions/` incluye extensiones útiles para arrays y strings.