/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.Core/Services/Shop/Interfaces/IOrdersService.cs
Descripción: Contrato para operaciones de pedidos. Incluye consultas con Include, creación con detalles, actualización parcial y eliminación (con ajuste de stock).
*/

using MiniERP_Suministros.Core.Models.Shop;

namespace MiniERP_Suministros.Core.Services.Shop
{
    public class OrderItemDraft
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Discount { get; set; }
    }

    public interface IOrdersService
    {
        IEnumerable<Order> GetAllOrdersData();
        IEnumerable<Order> GetAllOrdersDataByCashier(string cashierId);
        Order? GetById(int id);

        /// <summary>Crea un pedido con encabezado y detalles. Descuenta stock.</summary>
        Order Create(string cashierId, int customerId, decimal discount, string? comments, IEnumerable<OrderItemDraft> items);

        /// <summary>Actualiza parcialmente el encabezado del pedido.</summary>
        Order UpdatePartial(int id, decimal? discount = null, string? comments = null);

        /// <summary>Elimina un pedido devolviendo el stock de los productos.</summary>
        void Delete(int id);
    }
}
