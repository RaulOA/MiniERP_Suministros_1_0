/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.Server/ViewModels/Shop/ProductPatchVM.cs
Descripción: ViewModel para actualizaciones parciales de productos (PUT/PATCH). Todas las propiedades son anulables para distinguir "no enviado" de un valor explícito.
*/

namespace MiniERP_Suministros.Server.ViewModels.Shop
{
    public class ProductPatchVM
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public decimal? BuyingPrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public int? UnitsInStock { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDiscontinued { get; set; }

        // Relaciones (nullable para parches)
        public int? ProductCategoryId { get; set; }
        public int? ParentId { get; set; }
    }
}
