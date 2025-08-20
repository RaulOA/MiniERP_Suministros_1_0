
using FluentValidation;

namespace MiniERP_Suministros.Server.ViewModels.Shop
{
    public class ProductCategoryVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
    }

    public class ProductCategoryViewModelValidator : AbstractValidator<ProductCategoryVM>
    {
        public ProductCategoryViewModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Category name cannot be empty");
        }
    }
}
