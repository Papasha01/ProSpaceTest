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
    public class OrderRepository : IOrderRepository
    {
        private readonly ShopDbContext _context;

        public OrderRepository(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateOrder(Order order)
        {

            var orderEntity = new OrderEntity
            {
                Id = Guid.NewGuid(),
                CustomerId = order.CustomerId,
                OrderDate = DateTime.UtcNow,
                ShipmentDate = order.ShipmentDate,
                OrderNumber = order.OrderNumber,
                Status = order.Status,
                OrderItems = order.OrderItems.Select(oi => new OrderItemEntity
                {
                    ItemId = oi.ItemId,
                    ItemsCount = oi.ItemsCount,
                    ItemPrice = oi.ItemPrice
                }).ToList()
            };

            await _context.Order.AddAsync(orderEntity);
            await _context.SaveChangesAsync();

            return orderEntity.Id;
        }

        public async Task<Guid> DeleteOrder(Guid id)
        {
            var orderEntity = await _context.Order.FindAsync(id);
            if (orderEntity == null) return Guid.Empty;

            _context.Order.Remove(orderEntity);
            await _context.SaveChangesAsync();

            return orderEntity.Id;
        }

        public async Task<List<Order>> GetAllOrder()
        {
            return await _context.Order
                .Include(o => o.OrderItems)
                .Include(o => o.Customer)
                .Select(o => new Order
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    OrderDate = o.OrderDate,
                    ShipmentDate = o.ShipmentDate,
                    OrderNumber = o.OrderNumber,
                    Status = o.Status,
                    OrderItems = o.OrderItems.Select(oi => new OrderItem
                    {
                        ItemId = oi.ItemId,
                        ItemsCount = oi.ItemsCount,
                        ItemPrice = oi.ItemPrice
                    }).ToList()
                })
                .ToListAsync();
        }

        public Task<Order> GetOrderByCode(string code)
        {
            throw new NotSupportedException("Order don't have code property");
        }

        public async Task<Order> GetOrderById(Guid id)
        {
            var orderEntity = await _context.Order
                .Include(o => o.OrderItems)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orderEntity == null) return null;

            return new Order
            {
                Id = orderEntity.Id,
                CustomerId = orderEntity.CustomerId,
                OrderDate = orderEntity.OrderDate,
                ShipmentDate = orderEntity.ShipmentDate,
                OrderNumber = orderEntity.OrderNumber,
                Status = orderEntity.Status,
                OrderItems = orderEntity.OrderItems.Select(oi => new OrderItem
                {
                    ItemId = oi.ItemId,
                    ItemsCount = oi.ItemsCount,
                    ItemPrice = oi.ItemPrice
                }).ToList()
            };
        }

        public async Task<Guid> UpdateOrder(Order order)
        {
            var orderEntity = await _context.Order
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            if (orderEntity == null) return Guid.Empty;

            orderEntity.ShipmentDate = order.ShipmentDate;
            orderEntity.Status = order.Status;
            orderEntity.OrderNumber = order.OrderNumber;

            // Обновление элементов заказа
            orderEntity.OrderItems = order.OrderItems.Select(oi => new OrderItemEntity
            {
                OrderId = order.Id,
                ItemId = oi.ItemId,
                ItemsCount = oi.ItemsCount,
                ItemPrice = oi.ItemPrice
            }).ToList();

            await _context.SaveChangesAsync();
            return orderEntity.Id;
        }
    }
}