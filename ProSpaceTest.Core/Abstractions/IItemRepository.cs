using ProSpaceTest.Core.Models;

namespace ProSpaceTest.Core.Abstractions
{
    public interface IItemRepository
    {
        Task<List<Item>> GetAllItems();
        Task<Item> GetItemById(Guid id);
        Task<Item> GetItemByCode(string code);
        Task<Guid> CreateItem(Item item);
        Task<Guid> UpdateItem(Item item);
        Task<Guid> DeleteItem(Guid id);
    }
}
