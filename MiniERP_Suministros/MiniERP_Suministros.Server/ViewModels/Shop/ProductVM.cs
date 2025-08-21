/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.Server/ViewModels/Shop/ProductVM.cs
Descripción: ViewModel para productos expuestos por la API. Incluye validador con FluentValidation.
*/

using FluentValidation;

namespace MiniERP_Suministros.Server.ViewModels.Shop
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int UnitsInStock { get; set; }
        public bool IsActive { get; set; }
        public bool IsDiscontinued { get; set; }

        // Relaciones
        public int ProductCategoryId { get; set; }
        public int? ParentId { get; set; }

        // Campos de solo lectura para UI
        public string? ProductCategoryName { get; set; }
        public string? ParentName { get; set; }
    }

    public class ProductViewModelValidator : AbstractValidator<ProductVM>
    {
        public ProductViewModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name cannot be empty").MaximumLength(200);
            RuleFor(x => x.BuyingPrice).GreaterThanOrEqualTo(0);
            RuleFor(x => x.SellingPrice).GreaterThanOrEqualTo(0);
            RuleFor(x => x.UnitsInStock).GreaterThanOrEqualTo(0);
            // Para crear se debe especificar la categoría
            RuleFor(x => x.ProductCategoryId).GreaterThan(0).When(x => x.Id == 0);
        }
    }
}
