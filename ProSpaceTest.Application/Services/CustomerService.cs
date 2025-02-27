using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProSpaceTest.Core.Abstractions;
using ProSpaceTest.Core.Models;

namespace ProSpaceTest.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Guid> CreateCustomer(Core.Models.Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.Code))
                throw new ArgumentException("Customer code cannot be empty");

            return await _customerRepository.CreateCustomer(customer);
        }

        public async Task<Guid> DeleteCustomer(Guid id)
        {
            var result = await _customerRepository.DeleteCustomer(id);

            if (result == Guid.Empty)
                throw new KeyNotFoundException("Customer not found");

            return result;
        }

        public async Task<List<Core.Models.Customer>> GetAllCustomers()
        {
            return await _customerRepository.GetAllCustomers();
        }

        public async Task<Core.Models.Customer> GetCustomerByCode(string code)
        {
            var customer = await _customerRepository.GetCustomerByCode(code);

            if (customer == null)
                throw new KeyNotFoundException("Customer not found");

            return customer;
        }

        public async Task<Core.Models.Customer> GetCustomerById(Guid id)
        {
            var customer = await _customerRepository.GetCustomerById(id);

            if (customer == null)
                throw new KeyNotFoundException("Customer not found");

            return customer;
        }

        public async Task<Customer> GetCustomerByUserId(Guid id)
        {
            var customer = await _customerRepository.GetCustomerByUserId(id);

            if (customer == null)
                throw new KeyNotFoundException("Customer not found");

            return customer;
        }

        public async Task<Guid> UpdateCustomer(Core.Models.Customer customer)
        {
            // Базовые проверки перед обновлением
            if (customer.Id == Guid.Empty)
                throw new ArgumentException("Invalid customer ID");

            var result = await _customerRepository.UpdateCustomer(customer);

            if (result == Guid.Empty)
                throw new KeyNotFoundException("Customer not found");

            return result;
        }
    }
}