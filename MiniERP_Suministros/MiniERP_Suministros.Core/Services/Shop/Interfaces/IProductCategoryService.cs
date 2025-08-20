/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.Core/Services/Shop/Interfaces/IProductCategoryService.cs
Contrato para operaciones de categor�as de productos. Basado en ICustomerService: obtener, crear, actualizaci�n parcial y eliminaci�n.
*/

using MiniERP_Suministros.Core.Models.Shop;

namespace MiniERP_Suministros.Core.Services.Shop
{
    public interface IProductCategoryService
    {
        IEnumerable<ProductCategory> GetTopActiveCategories(int count);
        IEnumerable<ProductCategory> GetAllCategoriesData();

        ProductCategory? GetById(int id);

        /// <summary>
        /// Crea una nueva categor�a de productos.
        /// </summary>
        ProductCategory Create(string name, string? description, string? icon);

        /// <summary>
        /// Actualiza parcialmente una categor�a. Solo los valores no nulos son aplicados.
        /// </summary>
        ProductCategory UpdatePartial(int id,
            string? name = null,
            string? description = null,
            string? icon = null);

        /// <summary>
        /// Elimina una categor�a por id.
        /// </summary>
        void Delete(int id);
    }
}
