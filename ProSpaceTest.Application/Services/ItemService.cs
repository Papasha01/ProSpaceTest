using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ProSpaceTest.Core.Abstractions;
using ProSpaceTest.Core.Models;

namespace ProSpaceTest.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<Guid> CreateItem(Item item)
        {
            if (!Regex.IsMatch(item.Code, @"^\d{2}-\d{4}-[A-Z]{2}\d{2}$"))
            {
                throw new ArgumentException("Item code must be in format XX-XXXX-YYXX");
            }

            var existingItem = await _itemRepository.GetItemByCode(item.Code);
            if (existingItem != null)
            {
                throw new InvalidOperationException("Item with this code already exists");
            }

            return await _itemRepository.CreateItem(item);
        }

        public async Task<Guid> DeleteItem(Guid id)
        {
            var item = await _itemRepository.GetItemById(id);
            if (item == null)
            {
                throw new KeyNotFoundException("Item not found");
            }


            return await _itemRepository.DeleteItem(id);
        }

        public async Task<List<Item>> GetAllItems()
        {
            return await _itemRepository.GetAllItems();
        }

        public async Task<Item> GetItemByCode(string code)
        {
            var item = await _itemRepository.GetItemByCode(code);
            return item ?? throw new KeyNotFoundException("Item not found");
        }

        public async Task<Item> GetItemById(Guid id)
        {
            var item = await _itemRepository.GetItemById(id);
            return item ?? throw new KeyNotFoundException("Item not found");
        }

        public async Task<Guid> UpdateItem(Item item)
        {
            if (!Regex.IsMatch(item.Code, @"^\d{2}-\d{4}-[A-Z]{2}\d{2}$"))
            {
                throw new ArgumentException("Item code must be in format XX-XXXX-YYXX");
            }

            var existingItem = await _itemRepository.GetItemByCode(item.Code);
            if (existingItem != null && existingItem.Id != item.Id)
            {
                throw new InvalidOperationException("Item code must be unique");
            }

            var currentItem = await _itemRepository.GetItemById(item.Id);
            if (currentItem == null)
            {
                throw new KeyNotFoundException("Item not found");
            }

            return await _itemRepository.UpdateItem(item);
        }
    }
}