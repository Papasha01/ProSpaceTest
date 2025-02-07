using Microsoft.AspNetCore.Mvc;
using ProSpaceTest.API.Contracts;
using ProSpaceTest.Core.Abstractions;
using ProSpaceTest.Core.Models;

namespace ProSpaceTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerResponse>>> GetAll()
        {
            var customers = await _customerService.GetAllCustomers();
            var response = customers.Select(c => new CustomerResponse(
                c.Id,
                c.Name,
                c.Code,
                c.Address,
                c.Discount));

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetById(Guid id)
        {
            try
            {
                var customer = await _customerService.GetCustomerById(id);
                var response = new CustomerResponse(
                    customer.Id,
                    customer.Name,
                    customer.Code,
                    customer.Address,
                    customer.Discount);

                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Customer not found");
            }
        }

        [HttpGet("by-code/{code}")]
        public async Task<ActionResult<CustomerResponse>> GetByCode(string code)
        {
            try
            {
                var customer = await _customerService.GetCustomerByCode(code);
                var response = new CustomerResponse(
                    customer.Id,
                    customer.Name,
                    customer.Code,
                    customer.Address,
                    customer.Discount);

                return Ok(response);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Customer not found");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var (customer, error) = Customer.Create(
                    Guid.NewGuid(),
                    request.Name,
                    request.Code,
                    request.Address,
                    request.Discount);

                if (!string.IsNullOrEmpty(error))
                {
                    return BadRequest(error);
                }

                var id = await _customerService.CreateCustomer(customer);
                return CreatedAtAction(nameof(GetById), new { id }, id);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CustomerRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var customer = new Core.Models.Customer
                {
                    Id = id,
                    Name = request.Name,
                    Code = request.Code,
                    Address = request.Address,
                    Discount = request.Discount
                };

                var updatedId = await _customerService.UpdateCustomer(customer);
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
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _customerService.DeleteCustomer(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Customer not found");
            }
        }
    }
}