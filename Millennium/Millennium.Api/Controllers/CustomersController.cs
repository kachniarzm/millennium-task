using Microsoft.AspNetCore.Mvc;
using Millennium.Api.Models;
using Millennium.Api.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Millennium.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ILogger<CustomersController> _logger;
    private readonly ICustomersRepository _customersRepository;

    public CustomersController(ILogger<CustomersController> logger, ICustomersRepository customersRepository)
    {
        _logger = logger;
        _customersRepository = customersRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Customer>> Get()
    {
        var customers = _customersRepository.GetAll();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public ActionResult<Customer> Get(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid customer ID.");
        }

        var customer = _customersRepository.GetById(id);
        if (customer == null)
        {
            return NotFound($"Customer with ID {id} does not exist.");
        }
        return Ok(customer);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public ActionResult<Customer> Post([FromBody, Required] CustomerAddEdit customer)
    {
        if (customer == null)
        {
            return BadRequest("Customer data is required.");
        }

        var createdCustomer = _customersRepository.Create(customer);
        _logger.LogInformation($"Customer with ID {createdCustomer.Id} created.");
        return CreatedAtAction(nameof(Get), new { id = createdCustomer.Id }, createdCustomer);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public ActionResult<Customer> Put(int id, [FromBody, Required] CustomerAddEdit customer)
    {
        if (id <= 0 || customer == null)
        {
            return BadRequest("Invalid request parameters.");
        }

        var updatedCustomer = _customersRepository.Update(id, customer);
        if (updatedCustomer == null)
        {
            var message = $"Customer with ID {id} not found for update.";
            _logger.LogWarning(message);
            return NotFound(message);
        }

        _logger.LogInformation($"Customer with ID {id} updated.");
        return Ok(updatedCustomer);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public IActionResult Delete(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Invalid customer ID.");
        }

        if (!_customersRepository.Delete(id))
        {
            var message = $"Customer with ID {id} not found for deletion.";
            _logger.LogWarning(message);
            return NotFound(message);
        }

        _logger.LogInformation($"Customer with ID {id} deleted.");
        return NoContent();
    }
}
