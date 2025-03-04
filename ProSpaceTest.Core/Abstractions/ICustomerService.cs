﻿using ProSpaceTest.Core.Models;

namespace ProSpaceTest.Core.Abstractions
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomers();
        Task<Customer> GetCustomerById(Guid id);
        Task<Customer> GetCustomerByUserId(Guid id);
        Task<Customer> GetCustomerByCode(string code);
        Task<Guid> CreateCustomer(Customer customer);
        Task<Guid> UpdateCustomer(Customer customer);
        Task<Guid> DeleteCustomer(Guid id);
    }
}
