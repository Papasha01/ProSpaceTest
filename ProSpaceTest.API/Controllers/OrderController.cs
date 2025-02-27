using Microsoft.AspNetCore.Mvc;
using ProSpaceTest.API.Contracts;
using ProSpaceTest.Core.Abstractions;
using ProSpaceTest.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProSpaceTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IItemService _itemService;

        public OrderController(IOrderService orderService, IItemService itemService)
        {
            _orderService = orderService;
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAll()
        {
            try
            {
                var orders = await _orderService.GetAllOrder();
                var response = orders.Select(o => new OrderResponse(
                    o.Id,
                    o.CustomerId,
                    o.OrderDate,
                    o.ShipmentDate,
                    o.OrderNumber,
                    o.Status,
                    o.OrderItems.Select(oi => new OrderItemResponse(
                        oi.ItemId,
                        oi.ItemsCount,
                        oi.ItemPrice)).ToList()));

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> GetById(Guid id)
        {
            try
            {
                var order = await _orderService.GetOrderById(id);
                var response = new OrderResponse(
                    order.Id,
                    order.CustomerId,
                    order.OrderDate,
                    order.ShipmentDate,
                    order.OrderNumber,
                    order.Status,
                    order.OrderItems.Select(oi => new OrderItemResponse(
                        oi.ItemId,
                        oi.ItemsCount,
                        oi.ItemPrice)).ToList());

                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Order not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    CustomerId = request.CustomerId,
                    OrderDate = request.OrderDate,
                    ShipmentDate = request.ShipmentDate,
                    OrderNumber = request.OrderNumber,
                    Status = request.Status,
                    OrderItems = new List<OrderItem>()
                };

                foreach (var itemRequest in request.OrderItems)
                {
                    var item = await _itemService.GetItemById(itemRequest.ItemId);
                    if (item == null)
                    {
                        return NotFound($"Item with ID {itemRequest.ItemId} not found");
                    }

                    var orderItem = new OrderItem
                    {
                        ItemId = itemRequest.ItemId,
                        ItemsCount = itemRequest.ItemsCount,
                        ItemPrice = item.Price 
                    };

                    order.OrderItems.Add(orderItem);
                }

                var id = await _orderService.CreateOrder(order);
                return CreatedAtAction(nameof(GetById), new { id }, id);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] OrderRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var order = new Order
                {
                    Id = id,
                    CustomerId = request.CustomerId,
                    OrderDate = request.OrderDate,
                    ShipmentDate = request.ShipmentDate,
                    OrderNumber = request.OrderNumber,
                    Status = request.Status,
                    OrderItems = request.OrderItems.Select(oi => new OrderItem
                    {
                        ItemId = oi.ItemId,
                        ItemsCount = oi.ItemsCount,
                        ItemPrice = oi.ItemPrice
                        
                    }).ToList()
                };

                var updatedId = await _orderService.UpdateOrder(order);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _orderService.DeleteOrder(id);
                return Accepted();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Order not found");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}