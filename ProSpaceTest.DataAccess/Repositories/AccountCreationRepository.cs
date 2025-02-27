using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProSpaceTest.Core.Abstractions;
using ProSpaceTest.Core.Models;
using ProSpaceTest.DataAccess.Entites;

namespace ProSpaceTest.DataAccess.Repository
{
    public class AccountCreationRepository : IAccountCreationRepository
    {
        private readonly ShopDbContext _context;

        public AccountCreationRepository(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAccount(Customer customer, User user)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if(user.Role != "Admin" && user.Role != "Customer")
                {
                    throw new InvalidOperationException("Specify an existing role (Admin or Customer)");
                }
                if(await _context.User.AnyAsync(u => u.Login == user.Login))
                {
                    throw new InvalidOperationException("User with this login already exists");
                }
                var userEntity = new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Login = user.Login,
                    Password = HashPassword(user.Password),
                    Role = user.Role,
                    IsActive = user.IsActive
                };
                await _context.AddAsync(userEntity);
                await _context.SaveChangesAsync();
                
                if (user.Role == "Customer")
                {

                    if (await _context.Customer.AnyAsync(c => c.Code == customer.Code))
                    {
                        throw new InvalidOperationException("Customer with this code already exists");
                    }

                    var customerEntity = new CustomerEntity
                    {
                        Id = Guid.NewGuid(),
                        UserId = userEntity.Id,
                        Name = customer.Name,
                        Code = customer.Code,
                        Address = customer.Address,
                        Discount = customer.Discount
                    };

                    await _context.Customer.AddAsync(customerEntity);
                    await _context.SaveChangesAsync();
                }
                await transaction.CommitAsync();
                return user.Id;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

    }
}