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
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemResponse>>> GetAll()
        {
            try
            {
                var items = await _itemService.GetAllItems();
                var response = items.Select(i => new ItemResponse(
                    i.Id,
                    i.Code,
                    i.Name,
                    i.Price,
                    i.Category));

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemResponse>> GetById(Guid id)
        {
            try
            {
                var item = await _itemService.GetItemById(id);
                return Ok(new ItemResponse(
                    item.Id,
                    item.Code,
                    item.Name,
                    item.Price,
                    item.Category));
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Item not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("by-code/{code}")]
        public async Task<ActionResult<ItemResponse>> GetByCode(string code)
        {
            try
            {
                var item = await _itemService.GetItemByCode(code);
                return Ok(new ItemResponse(
                    item.Id,
                    item.Code,
                    item.Name,
                    item.Price,
                    item.Category));
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Item not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ItemRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var (item, error) = Item.Create(
                    Guid.NewGuid(),
                    request.Code,
                    request.Name,
                    request.Price,
                    request.Category);

                if (!string.IsNullOrEmpty(error))
                {
                    return BadRequest(error);
                }

                var id = await _itemService.CreateItem(item);
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
        public async Task<IActionResult> Update(Guid id, [FromBody] ItemRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = new Item
                {
                    Id = id,
                    Code = request.Code,
                    Name = request.Name,
                    Price = request.Price,
                    Category = request.Category
                };

                var updatedId = await _itemService.UpdateItem(item);
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
                await _itemService.DeleteItem(id);
                return Accepted();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Item not found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}