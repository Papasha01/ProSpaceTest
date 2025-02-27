using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProSpaceTest.Core.Abstractions;
using ProSpaceTest.Core.Models;
using ProSpaceTest.DataAccess.Repository;

namespace ProSpaceTest.Application.Services
{
    public class AccountCreationService : IAccountCreationService
    {
        private readonly IAccountCreationRepository _accountCreationRepository;

        public AccountCreationService(IAccountCreationRepository accountCreationRepository)
        {
            _accountCreationRepository = accountCreationRepository;
        }

        public async Task<Guid> CreateAccount(Customer customer, User user)
        {
            try
            {
                var id = await _accountCreationRepository.CreateAccount(customer, user);
                return id;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}