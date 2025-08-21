/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.Server/ViewModels/Shop/OrderItemVM.cs
Descripci�n: ViewModel de detalle de pedido para exponer l�neas en la API (lectura).
*/

namespace MiniERP_Suministros.Server.ViewModels.Shop
{
    public class OrderItemVM
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal LineTotal => (UnitPrice - Discount) * Quantity;
    }
}
