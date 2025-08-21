/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.Core/Services/Shop/Interfaces/IProductService.cs
Descripción: Contrato para operaciones de productos. Incluye consultas con Include, creación, actualización parcial y eliminación.
*/

using MiniERP_Suministros.Core.Models.Shop;

namespace MiniERP_Suministros.Core.Services.Shop
{
    public interface IProductService
    {
        IEnumerable<Product> GetTopActiveProducts(int count);
        IEnumerable<Product> GetAllProductsData();

        Product? GetById(int id);

        /// <summary>
        /// Crea un nuevo producto con los datos proporcionados.
        /// </summary>
        Product Create(
            string name,
            string? description,
            string? icon,
            decimal buyingPrice,
            decimal sellingPrice,
            int unitsInStock,
            bool isActive,
            bool isDiscontinued,
            int productCategoryId,
            int? parentId);

        /// <summary>
        /// Actualiza parcialmente un producto. Solo los valores no nulos son aplicados.
        /// </summary>
        Product UpdatePartial(
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
            int? parentId = null);

        /// <summary>
        /// Elimina un producto por id.
        /// </summary>
        void Delete(int id);
    }
}
