/*
RUTA: MiniERP_Suministros/MiniERP_Suministros.Core/Services/Shop/CustomerService.cs
Implementación de ICustomerService. Se agregan métodos de consulta, creación, actualización parcial y eliminación.
*/

using System;
using Microsoft.EntityFrameworkCore;
using MiniERP_Suministros.Core.Infrastructure;
using MiniERP_Suministros.Core.Models;
using MiniERP_Suministros.Core.Models.Shop;

namespace MiniERP_Suministros.Core.Services.Shop
{
    public class CustomerService(ApplicationDbContext dbContext) : ICustomerService
    {
        public IEnumerable<Customer> GetTopActiveCustomers(int count) => throw new NotImplementedException();

        public IEnumerable<Customer> GetAllCustomersData() => dbContext.Customers
                .Include(c => c.Orders).ThenInclude(o => o.OrderDetails).ThenInclude(d => d.Product)
                .Include(c => c.Orders).ThenInclude(o => o.Cashier)
                .AsSingleQuery()
                .OrderBy(c => c.Name)
                .ToList();

        public Customer? GetById(int id) => dbContext.Customers.FirstOrDefault(c => c.Id == id);

        public Customer Create(string name, string email, string? phoneNumber, string? address, string? city, string? gender)
        {
            var entity = new Customer
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber,
                Address = address,
                City = city,
                Gender = ParseGender(gender)
            };

            dbContext.Customers.Add(entity); // audit se aplica en SaveChanges
            dbContext.SaveChanges();
            return entity;
        }

        public Customer UpdatePartial(int id,
            string? name = null,
            string? email = null,
            string? phoneNumber = null,
            string? address = null,
            string? city = null,
            string? gender = null)
        {
            var entity = dbContext.Customers.FirstOrDefault(c => c.Id == id)
                ?? throw new KeyNotFoundException($"Customer {id} not found");

            if (name != null) entity.Name = name;
            if (email != null) entity.Email = email;
            if (phoneNumber != null) entity.PhoneNumber = phoneNumber;
            if (address != null) entity.Address = address;
            if (city != null) entity.City = city;
            if (gender != null) entity.Gender = ParseGender(gender);

            dbContext.SaveChanges();
            return entity;
        }

        public void Delete(int id)
        {
            var entity = dbContext.Customers.FirstOrDefault(c => c.Id == id)
                ?? throw new KeyNotFoundException($"Customer {id} not found");

            dbContext.Customers.Remove(entity);
            dbContext.SaveChanges();
        }

        private static Gender ParseGender(string? gender)
        {
            if (string.IsNullOrWhiteSpace(gender)) return Gender.None;
            return Enum.TryParse<Gender>(gender, true, out var g) ? g : Gender.None;
        }
    }
}
