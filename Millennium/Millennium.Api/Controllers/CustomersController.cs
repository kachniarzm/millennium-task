using Microsoft.AspNetCore.Mvc;
using Millennium.Api.Models;
using Millennium.Api.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Millennium.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ILogger<CustomersController> _logger;
    private ICustomersRepository _customersRepository;

    public CustomersController(ILogger<CustomersController> logger, ICustomersRepository customersRepository)
    {
        _logger = logger;
        _customersRepository = customersRepository;
    }

    // GET: api/<CustomersController>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<List<Customer>> Get()
    {
        return _customersRepository.GetAll();
    }

    // GET api/<CustomersController>/5
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Customer> Get(int id)
    {
        var customer = _customersRepository.GetById(id);

        if (customer == null)
        {
            return NotFound($"Customer with id {id} does not exist.");
        }

        return customer;
    }

    // POST api/<CustomersController>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Customer> Post([FromBody] CustomerAddEdit customer)
    {
        var created = _customersRepository.Create(customer);

        _logger.LogInformation($"Customer with id {created.Id} created.");
        return created;
    }

    // PUT api/<CustomersController>/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Customer> Put(int id, [FromBody] CustomerAddEdit customer)
    {
        var updated = _customersRepository.Update(id, customer);

        if (updated == null)
        {
            var message = $"Customer with id {id} not found. Cannot be updated.";
            _logger.LogInformation(message);
            return NotFound(message);
        }

        _logger.LogInformation($"Customer with id {id} updated.");
        return updated;
    }

    // DELETE api/<CustomersController>/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Delete(int id)
    {
        if (_customersRepository.Delete(id))
        {
            _logger.LogInformation($"Customer with id {id} deleted.");
            return NoContent();
        }
        else
        {
            var message = $"Customer with id {id} not found. Cannot be deleted.";
            _logger.LogInformation(message);
            return NotFound(message);
        }
    }
}
