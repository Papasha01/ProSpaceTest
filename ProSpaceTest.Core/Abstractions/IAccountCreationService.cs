using ProSpaceTest.Core.Models;

namespace ProSpaceTest.Core.Abstractions
{
    public interface IAccountCreationService
    {
        Task<Guid> CreateAccount(Customer customer, User user);
    }
}
