/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.Core/Services/Shop/OrdersService.cs
Implementación de IOrdersService. Incluye consultas con Include, creación con detalles (descuenta stock), actualización parcial del encabezado y eliminación (restaura stock).
*/

using Microsoft.EntityFrameworkCore;
using MiniERP_Suministros.Core.Infrastructure;
using MiniERP_Suministros.Core.Models.Shop;

namespace MiniERP_Suministros.Core.Services.Shop
{
    public class OrdersService(ApplicationDbContext dbContext) : IOrdersService
    {
        public IEnumerable<Order> GetAllOrdersData() => dbContext.Orders
            .Include(o => o.Customer)
            .Include(o => o.Cashier)
            .Include(o => o.OrderDetails).ThenInclude(d => d.Product)
            .AsSingleQuery()
            .OrderByDescending(o => o.CreatedDate)
            .AsNoTracking()
            .ToList();

        public IEnumerable<Order> GetAllOrdersDataByCashier(string cashierId) => dbContext.Orders
            .Include(o => o.Customer)
            .Include(o => o.Cashier)
            .Include(o => o.OrderDetails).ThenInclude(d => d.Product)
            .Where(o => o.CashierId == cashierId)
            .AsSingleQuery()
            .OrderByDescending(o => o.CreatedDate)
            .AsNoTracking()
            .ToList();

        public Order? GetById(int id) => dbContext.Orders
            .Include(o => o.Customer)
            .Include(o => o.Cashier)
            .Include(o => o.OrderDetails).ThenInclude(d => d.Product)
            .AsSingleQuery()
            .FirstOrDefault(o => o.Id == id);

        public Order Create(string cashierId, int customerId, decimal discount, string? comments, IEnumerable<OrderItemDraft> items)
        {
            if (string.IsNullOrWhiteSpace(cashierId)) throw new ArgumentException("cashierId is required");
            if (!items?.Any() ?? true) throw new ArgumentException("At least one order item is required");

            var customer = dbContext.Customers.FirstOrDefault(c => c.Id == customerId)
                ?? throw new KeyNotFoundException($"Customer {customerId} not found");

            // Cargar productos implicados
            var productIds = items.Select(i => i.ProductId).Distinct().ToArray();
            var products = dbContext.Products.Where(p => productIds.Contains(p.Id)).ToDictionary(p => p.Id);
            if (products.Count != productIds.Length) throw new InvalidOperationException("One or more products were not found");

            // Validaciones básicas y ajuste de stock
            foreach (var item in items)
            {
                var product = products[item.ProductId];
                if (!product.IsActive || product.IsDiscontinued) throw new InvalidOperationException($"Product {product.Id} is not available");
                if (item.Quantity <= 0) throw new ArgumentException("Quantity must be > 0");
                if (product.UnitsInStock < item.Quantity) throw new InvalidOperationException($"Insufficient stock for product {product.Name}");
            }

            var order = new Order
            {
                CashierId = cashierId,
                Customer = customer,
                Discount = discount < 0 ? 0 : discount,
                Comments = comments
            };

            foreach (var item in items)
            {
                var product = products[item.ProductId];
                var unitPrice = item.UnitPrice ?? product.SellingPrice;
                var lineDiscount = item.Discount ?? 0m;
                if (lineDiscount < 0 || lineDiscount > unitPrice) throw new ArgumentException("Invalid line discount");

                // Descontar stock
                product.UnitsInStock -= item.Quantity;

                var detail = new OrderDetail
                {
                    Product = product,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice,
                    Discount = lineDiscount,
                    Order = order
                };

                dbContext.OrderDetails.Add(detail);
            }

            dbContext.Orders.Add(order);
            dbContext.SaveChanges();

            return order;
        }

        public Order UpdatePartial(int id, decimal? discount = null, string? comments = null)
        {
            var order = dbContext.Orders.FirstOrDefault(o => o.Id == id)
                ?? throw new KeyNotFoundException($"Order {id} not found");

            if (discount.HasValue && discount.Value < 0) throw new ArgumentException("Invalid order discount");
            if (discount.HasValue) order.Discount = discount.Value;
            if (comments != null) order.Comments = comments;

            dbContext.SaveChanges();
            return order;
        }

        public void Delete(int id)
        {
            var order = dbContext.Orders
                .Include(o => o.OrderDetails).ThenInclude(d => d.Product)
                .FirstOrDefault(o => o.Id == id)
                ?? throw new KeyNotFoundException($"Order {id} not found");

            // Restaurar stock
            foreach (var d in order.OrderDetails)
            {
                d.Product.UnitsInStock += d.Quantity;
            }

            dbContext.OrderDetails.RemoveRange(order.OrderDetails);
            dbContext.Orders.Remove(order);
            dbContext.SaveChanges();
        }
    }
}
