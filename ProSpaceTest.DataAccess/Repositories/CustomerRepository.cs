using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProSpaceTest.Core.Abstractions;
using ProSpaceTest.Core.Models;
using ProSpaceTest.DataAccess.Entites;

namespace ProSpaceTest.DataAccess.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ShopDbContext _context;

        public CustomerRepository(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateCustomer(Core.Models.Customer customer)
        {
            // Валидация кода клиента
            if (await _context.Customer.AnyAsync(c => c.Code == customer.Code))
            {
                throw new InvalidOperationException("Customer with this code already exists");
            }


            var customerEntity = new CustomerEntity
            {
                Id = Guid.NewGuid(),
                Name = customer.Name,
                Code = customer.Code,
                Address = customer.Address,
                Discount = customer.Discount
            };

            await _context.Customer.AddAsync(customerEntity);
            await _context.SaveChangesAsync();

            return customerEntity.Id;
        }

        public async Task<Guid> DeleteCustomer(Guid id)
        {
            var customerEntity = await _context.Customer.FindAsync(id);
            if (customerEntity == null)
            {
                return Guid.Empty;
            }

            _context.Customer.Remove(customerEntity);
            await _context.SaveChangesAsync();

            return customerEntity.Id;
        }

        public async Task<List<Core.Models.Customer>> GetAllCustomers()
        {
            var customerEntities = await _context.Customer
                .AsNoTracking()
                .ToListAsync();

            return customerEntities
                .Select(c => Core.Models.Customer.Create(
                    c.Id,
                    c.Name,
                    c.Code,
                    c.Address,
                    c.Discount).Customer)
                .ToList();
        }

        public async Task<Core.Models.Customer> GetCustomerByCode(string code)
        {
            var customerEntity = await _context.Customer
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Code == code);

            if (customerEntity == null)
            {
                return null;
            }

            return Core.Models.Customer.Create(
                customerEntity.Id,
                customerEntity.Name,
                customerEntity.Code,
                customerEntity.Address,
                customerEntity.Discount).Customer;
        }

        public async Task<Core.Models.Customer> GetCustomerById(Guid id)
        {
            var customerEntity = await _context.Customer
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customerEntity == null)
            {
                return null;
            }

            return Core.Models.Customer.Create(
                customerEntity.Id,
                customerEntity.Name,
                customerEntity.Code,
                customerEntity.Address,
                customerEntity.Discount).Customer;
        }

        public async Task<Guid> UpdateCustomer(Core.Models.Customer customer)
        {
            var customerEntity = await _context.Customer.FindAsync(customer.Id);
            if (customerEntity == null)
            {
                return Guid.Empty;
            }

            // Проверка уникальности кода
            if (await _context.Customer.AnyAsync(c => c.Code == customer.Code && c.Id != customer.Id))
            {
                throw new InvalidOperationException("Customer code must be unique");
            }

            customerEntity.Name = customer.Name;
            customerEntity.Code = customer.Code;
            customerEntity.Address = customer.Address;
            customerEntity.Discount = customer.Discount;

            _context.Customer.Update(customerEntity);
            await _context.SaveChangesAsync();

            return customerEntity.Id;
        }
    }
}