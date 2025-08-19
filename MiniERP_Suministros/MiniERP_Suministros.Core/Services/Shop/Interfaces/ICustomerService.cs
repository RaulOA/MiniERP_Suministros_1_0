/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.Core/Services/Shop/Interfaces/ICustomerService.cs
Definición del contrato para operaciones de clientes. Se agregan métodos para obtener, crear, actualizar parcialmente y eliminar.
*/

using MiniERP_Suministros.Core.Models.Shop;

namespace MiniERP_Suministros.Core.Services.Shop
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetTopActiveCustomers(int count);
        IEnumerable<Customer> GetAllCustomersData();

        Customer? GetById(int id);

        /// <summary>
        /// Crea un nuevo cliente con los datos proporcionados.
        /// </summary>
        Customer Create(string name, string email, string? phoneNumber, string? address, string? city, string? gender);

        /// <summary>
        /// Actualiza parcialmente un cliente. Solo los valores no nulos son aplicados.
        /// </summary>
        Customer UpdatePartial(int id,
            string? name = null,
            string? email = null,
            string? phoneNumber = null,
            string? address = null,
            string? city = null,
            string? gender = null);

        /// <summary>
        /// Elimina un cliente por id.
        /// </summary>
        void Delete(int id);
    }
}
