





using MiniERP_Suministros.Core.Models.Shop;

namespace MiniERP_Suministros.Core.Services.Shop
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetTopActiveCustomers(int count);
        IEnumerable<Customer> GetAllCustomersData();
    }
}
