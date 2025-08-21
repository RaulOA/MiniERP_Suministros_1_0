/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.Server/ViewModels/Shop/OrderVM.cs
Descripción: ViewModels para pedidos. OrderVM (lectura) y OrderCreateVM (creación).
*/

namespace MiniERP_Suministros.Server.ViewModels.Shop
{
    public class OrderVM
    {
        public int Id { get; set; }
        public decimal Discount { get; set; }
        public string? Comments { get; set; }

        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CashierId { get; set; }
        public string? CashierName { get; set; }

        public DateTime CreatedDate { get; set; }
        public decimal Subtotal => Items?.Sum(i => i.UnitPrice * i.Quantity) ?? 0m;
        public decimal ItemsDiscount => Items?.Sum(i => i.Discount * i.Quantity) ?? 0m;
        public decimal Total => Subtotal - ItemsDiscount - Discount;

        public ICollection<OrderItemVM>? Items { get; set; }
    }

    public class OrderCreateItemVM
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Discount { get; set; }
    }

    public class OrderCreateVM
    {
        public int CustomerId { get; set; }
        public decimal Discount { get; set; }
        public string? Comments { get; set; }
        public ICollection<OrderCreateItemVM> Items { get; set; } = new List<OrderCreateItemVM>();
    }
}
