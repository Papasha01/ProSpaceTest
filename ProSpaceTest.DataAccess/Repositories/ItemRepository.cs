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
    public class ItemRepository : IItemRepository
    {
        private readonly ShopDbContext _context;

        public ItemRepository(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateItem(Core.Models.Item item)
        {
            // Проверка уникальности кода товара
            if (await _context.Item.AnyAsync(i => i.Code == item.Code))
            {
                throw new InvalidOperationException("Item with this code already exists");
            }

            var itemEntity = new ItemEntity
            {
                Id = Guid.NewGuid(),
                Code = item.Code,
                Name = item.Name,
                Price = item.Price,
                Category = item.Category
            };

            await _context.Item.AddAsync(itemEntity);
            await _context.SaveChangesAsync();

            return itemEntity.Id;
        }

        public async Task<Guid> DeleteItem(Guid id)
        {
            var itemEntity = await _context.Item.FindAsync(id);
            if (itemEntity == null)
            {
                return Guid.Empty;
            }

            _context.Item.Remove(itemEntity);
            await _context.SaveChangesAsync();

            return itemEntity.Id;
        }

        public async Task<List<Core.Models.Item>> GetAllItems()
        {
            var items = await _context.Item
                .AsNoTracking()
                .ToListAsync();

            return items.Select(i => new Core.Models.Item
            {
                Id = i.Id,
                Code = i.Code,
                Name = i.Name,
                Price = i.Price,
                Category = i.Category
            }).ToList();
        }

        public async Task<Core.Models.Item> GetItemByCode(string code)
        {
            var itemEntity = await _context.Item
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Code == code);

            if (itemEntity == null) return null;

            return new Core.Models.Item
            {
                Id = itemEntity.Id,
                Code = itemEntity.Code,
                Name = itemEntity.Name,
                Price = itemEntity.Price,
                Category = itemEntity.Category
            };
        }

        public async Task<Core.Models.Item> GetItemById(Guid id)
        {
            var itemEntity = await _context.Item
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);

            if (itemEntity == null) return null;

            return new Core.Models.Item
            {
                Id = itemEntity.Id,
                Code = itemEntity.Code,
                Name = itemEntity.Name,
                Price = itemEntity.Price,
                Category = itemEntity.Category
            };
        }

        public async Task<Guid> UpdateItem(Core.Models.Item item)
        {
            var itemEntity = await _context.Item.FindAsync(item.Id);
            if (itemEntity == null)
            {
                return Guid.Empty;
            }

            // Проверка уникальности кода при изменении
            if (itemEntity.Code != item.Code &&
                await _context.Item.AnyAsync(i => i.Code == item.Code))
            {
                throw new InvalidOperationException("Item code must be unique");
            }

            itemEntity.Code = item.Code;
            itemEntity.Name = item.Name;
            itemEntity.Price = item.Price;
            itemEntity.Category = item.Category;

            _context.Item.Update(itemEntity);
            await _context.SaveChangesAsync();

            return itemEntity.Id;
        }
    }
}