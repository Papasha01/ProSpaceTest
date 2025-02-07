using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProSpaceTest.Core.Abstractions;
using ProSpaceTest.Core.Models;

namespace ProSpaceTest.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Guid> CreateOrder(Order order)
        {
            if (order.OrderDate > DateTime.UtcNow)
            {
                throw new ArgumentException("Order date cannot be in the future.");
            }

            if (order.OrderItems == null || !order.OrderItems.Any())
            {
                throw new ArgumentException("Order must contain at least one item.");
            }

            var existingOrder = await _orderRepository.GetOrderById(order.Id);
            if (existingOrder != null)
            {
                throw new InvalidOperationException("Order with this ID already exists.");
            }

            foreach (var item in order.OrderItems)
            {
                if(item.ItemsCount <= 0)
                    throw new InvalidOperationException("Items count must contain at least one item count.");
            }

            return await _orderRepository.CreateOrder(order);
        }

        public async Task<Guid> DeleteOrder(Guid id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            // Проверка статуса заказа перед удалением
            if (order.Status == "Completed")
            {
                throw new InvalidOperationException("Cannot delete a completed order.");
            }

            return await _orderRepository.DeleteOrder(id);
        }

        public async Task<List<Order>> GetAllOrder()
        {
            return await _orderRepository.GetAllOrder();
        }

        public Task<Order> GetOrderByCode(string code)
        {
            throw new NotSupportedException("Orders don't have code property");
        }

        public async Task<Order> GetOrderById(Guid id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            return order;
        }

        public async Task<Guid> UpdateOrder(Order order)
        {
            var existingOrder = await _orderRepository.GetOrderById(order.Id);
            if (existingOrder == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            // Проверка статуса заказа перед обновлением
            if (existingOrder.Status == "Completed")
            {
                throw new InvalidOperationException("Cannot update a completed order.");
            }

            // Валидация даты доставки
            
            if (order.ShipmentDate.HasValue && order.ShipmentDate < order.OrderDate)
            {
                throw new ArgumentException("Shipment date cannot be before order date.");
            }

            return await _orderRepository.UpdateOrder(order);
        }
    }
}