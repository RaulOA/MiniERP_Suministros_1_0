/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.Core/Services/Shop/ProductCategoryService.cs
Implementación de IProductCategoryService. Métodos de consulta con relaciones, crear, actualizar parcial y eliminar.
*/

using Microsoft.EntityFrameworkCore;
using MiniERP_Suministros.Core.Infrastructure;
using MiniERP_Suministros.Core.Models.Shop;

namespace MiniERP_Suministros.Core.Services.Shop
{
    public class ProductCategoryService(ApplicationDbContext dbContext) : IProductCategoryService
    {
        public IEnumerable<ProductCategory> GetTopActiveCategories(int count) => dbContext.ProductCategories
            .Include(c => c.Products)
            .OrderBy(c => c.Name)
            .Take(count)
            .AsNoTracking()
            .ToList();

        public IEnumerable<ProductCategory> GetAllCategoriesData() => dbContext.ProductCategories
            .Include(c => c.Products)
                .ThenInclude(p => p.OrderDetails)
            .AsSingleQuery()
            .OrderBy(c => c.Name)
            .AsNoTracking()
            .ToList();

        public ProductCategory? GetById(int id) => dbContext.ProductCategories
            .Include(c => c.Products)
            .AsSingleQuery()
            .FirstOrDefault(c => c.Id == id);

        public ProductCategory Create(string name, string? description, string? icon)
        {
            var entity = new ProductCategory
            {
                Name = name,
                Description = description,
                Icon = icon
            };

            dbContext.ProductCategories.Add(entity); // audit se aplica en SaveChanges
            dbContext.SaveChanges();
            return entity;
        }

        public ProductCategory UpdatePartial(int id, string? name = null, string? description = null, string? icon = null)
        {
            var entity = dbContext.ProductCategories.FirstOrDefault(c => c.Id == id)
                ?? throw new KeyNotFoundException($"ProductCategory {id} not found");

            if (name != null) entity.Name = name;
            if (description != null) entity.Description = description;
            if (icon != null) entity.Icon = icon;

            dbContext.SaveChanges();
            return entity;
        }

        public void Delete(int id)
        {
            var entity = dbContext.ProductCategories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.Id == id)
                ?? throw new KeyNotFoundException($"ProductCategory {id} not found");

            if (entity.Products.Any())
            {
                // Evitar eliminar si tiene productos asociados. Política básica; ajustar según reglas.
                throw new InvalidOperationException("Cannot delete category with related products.");
            }

            dbContext.ProductCategories.Remove(entity);
            dbContext.SaveChanges();
        }
    }
}
