using ProSpaceTest.Core.Models;

namespace ProSpaceTest.Core.Abstractions
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrder();
        Task<Order> GetOrderById(Guid id);
        Task<Order> GetOrderByCode(string code);
        Task<Guid> CreateOrder(Order order);
        Task<Guid> UpdateOrder(Order order);
        Task<Guid> DeleteOrder(Guid id);
    }
}
