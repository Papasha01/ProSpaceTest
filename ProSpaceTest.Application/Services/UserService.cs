using System;
using System.Threading.Tasks;
using ProSpaceTest.Core.Abstractions;
using ProSpaceTest.Core.Models;
using ProSpaceTest.DataAccess.Repository;

namespace ProSpaceTest.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Authenticate(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Login and password are required.");
            }

            var user = await _userRepository.Authenticate(login, password);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid login or password.");
            }

            return user;
        }

        public async Task<Guid> CreateUser(User user)
        {
            return await _userRepository.CreateUser(user);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task<Guid> DeleteUser(Guid id)
        {
            return await _userRepository.DeleteUser(id);
        }

        public Task<Guid> UpdateUser(User user)
        {
            return _userRepository.UpdateUser(user);
        }

        public async Task<User> GetUserById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("User ID is required.");
            }

            var user = await _userRepository.GetUserById(id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            return user;
        }

    }
}