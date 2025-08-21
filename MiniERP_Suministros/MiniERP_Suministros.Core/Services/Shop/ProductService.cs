/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.Core/Services/Shop/ProductService.cs
Implementación de IProductService. Consultas con relaciones, crear, actualizar parcial y eliminar. Incluye validaciones básicas.
*/

using Microsoft.EntityFrameworkCore;
using MiniERP_Suministros.Core.Infrastructure;
using MiniERP_Suministros.Core.Models.Shop;

namespace MiniERP_Suministros.Core.Services.Shop
{
    public class ProductService(ApplicationDbContext dbContext) : IProductService
    {
        public IEnumerable<Product> GetTopActiveProducts(int count) => dbContext.Products
            .Include(p => p.ProductCategory)
            .Where(p => p.IsActive && !p.IsDiscontinued)
            .OrderBy(p => p.Name)
            .Take(count)
            .AsNoTracking()
            .ToList();

        public IEnumerable<Product> GetAllProductsData() => dbContext.Products
            .Include(p => p.ProductCategory)
            .Include(p => p.Parent)
            .Include(p => p.Children)
            .Include(p => p.OrderDetails)
            .AsSingleQuery()
            .OrderBy(p => p.Name)
            .AsNoTracking()
            .ToList();

        public Product? GetById(int id) => dbContext.Products
            .Include(p => p.ProductCategory)
            .Include(p => p.Parent)
            .Include(p => p.Children)
            .Include(p => p.OrderDetails)
            .AsSingleQuery()
            .FirstOrDefault(p => p.Id == id);

        public Product Create(
            string name,
            string? description,
            string? icon,
            decimal buyingPrice,
            decimal sellingPrice,
            int unitsInStock,
            bool isActive,
            bool isDiscontinued,
            int productCategoryId,
            int? parentId)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ProductException("Product name is required");
            if (buyingPrice < 0 || sellingPrice < 0) throw new ProductException("Prices must be non-negative");
            if (unitsInStock < 0) throw new ProductException("UnitsInStock must be non-negative");

            var category = dbContext.ProductCategories.FirstOrDefault(c => c.Id == productCategoryId)
                ?? throw new ProductException($"ProductCategory {productCategoryId} not found");

            Product? parent = null;
            if (parentId.HasValue)
            {
                parent = dbContext.Products.FirstOrDefault(p => p.Id == parentId.Value)
                    ?? throw new ProductException($"Parent product {parentId.Value} not found");
            }

            var entity = new Product
            {
                Name = name,
                Description = description,
                Icon = icon,
                BuyingPrice = buyingPrice,
                SellingPrice = sellingPrice,
                UnitsInStock = unitsInStock,
                IsActive = isActive,
                IsDiscontinued = isDiscontinued,
                ProductCategory = category,
                Parent = parent
            };

            dbContext.Products.Add(entity);
            dbContext.SaveChanges();
            return entity;
        }

        public Product UpdatePartial(
            int id,
            string? name = null,
            string? description = null,
            string? icon = null,
            decimal? buyingPrice = null,
            decimal? sellingPrice = null,
            int? unitsInStock = null,
            bool? isActive = null,
            bool? isDiscontinued = null,
            int? productCategoryId = null,
            int? parentId = null)
        {
            var entity = dbContext.Products
                .Include(p => p.ProductCategory)
                .FirstOrDefault(p => p.Id == id)
                ?? throw new KeyNotFoundException($"Product {id} not found");

            if (name != null) entity.Name = name;
            if (description != null) entity.Description = description;
            if (icon != null) entity.Icon = icon;
            if (buyingPrice.HasValue)
            {
                if (buyingPrice.Value < 0) throw new ProductException("BuyingPrice must be non-negative");
                entity.BuyingPrice = buyingPrice.Value;
            }
            if (sellingPrice.HasValue)
            {
                if (sellingPrice.Value < 0) throw new ProductException("SellingPrice must be non-negative");
                entity.SellingPrice = sellingPrice.Value;
            }
            if (unitsInStock.HasValue)
            {
                if (unitsInStock.Value < 0) throw new ProductException("UnitsInStock must be non-negative");
                entity.UnitsInStock = unitsInStock.Value;
            }
            if (isActive.HasValue) entity.IsActive = isActive.Value;
            if (isDiscontinued.HasValue) entity.IsDiscontinued = isDiscontinued.Value;

            if (productCategoryId.HasValue)
            {
                var category = dbContext.ProductCategories.FirstOrDefault(c => c.Id == productCategoryId.Value)
                    ?? throw new ProductException($"ProductCategory {productCategoryId.Value} not found");
                entity.ProductCategory = category;
            }

            if (parentId.HasValue)
            {
                if (parentId.Value == 0)
                {
                    entity.Parent = null; // permitir quitar padre
                }
                else
                {
                    var parent = dbContext.Products.FirstOrDefault(p => p.Id == parentId.Value)
                        ?? throw new ProductException($"Parent product {parentId.Value} not found");
                    entity.Parent = parent;
                }
            }

            dbContext.SaveChanges();
            return entity;
        }

        public void Delete(int id)
        {
            var entity = dbContext.Products
                .Include(p => p.Children)
                .Include(p => p.OrderDetails)
                .FirstOrDefault(p => p.Id == id)
                ?? throw new KeyNotFoundException($"Product {id} not found");

            if (entity.Children.Any())
            {
                throw new ProductException("Cannot delete product with child products.");
            }
            if (entity.OrderDetails.Any())
            {
                throw new ProductException("Cannot delete product with order details.");
            }

            dbContext.Products.Remove(entity);
            dbContext.SaveChanges();
        }
    }
}
