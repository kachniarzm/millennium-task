using Millennium.Api.Controllers;
using Millennium.Api.Models;
using Millennium.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Millennium.UnitTests.Controllers;

internal class CustomersControllerUnitTests
{
    private readonly Mock<ILogger<CustomersController>> _mockLogger = new();

    [Test]
    public void WhenGetAllCustomers_AllCustomersAreReturned()
    {
        var mockRepo = new Mock<ICustomersRepository>();
        mockRepo.Setup(repo => repo.GetAll()).Returns(GetSampleCustomers());
        var controller = new CustomersController(_mockLogger.Object, mockRepo.Object);

        var result = controller.Get();

        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var customers = okResult.Value as IEnumerable<Customer>;
        Assert.That(customers, Is.Not.Null); 
        Assert.That(customers.Count(), Is.EqualTo(3));
    }

    [Test]
    public void WhenGetByIdCustomerWithWrongId_NotFoundIsReturned()
    {
        var mockRepo = new Mock<ICustomersRepository>();
        mockRepo.Setup(repo => repo.GetById(1)).Returns((Customer?)null);
        var controller = new CustomersController(_mockLogger.Object, mockRepo.Object);

        var result = controller.Get(1);

        Assert.That(result.Result is NotFoundObjectResult);
    }

    [Test]
    public void WhenGetByIdCustomer_CustomerIsReturned()
    {
        var mockRepo = new Mock<ICustomersRepository>();
        mockRepo.Setup(repo => repo.GetById(1))
            .Returns(new Customer() { Id = 1, FirstName = "Delbert", LastName = "Traver" });
        var controller = new CustomersController(_mockLogger.Object, mockRepo.Object);

        var result = controller.Get(1);

        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null, "Expected OkObjectResult but got null");
        var customer = okResult.Value as Customer;
        Assert.That(customer, Is.Not.Null, "Expected Customer object but got null");
        Assert.That(customer.Id, Is.EqualTo(1));
        Assert.That(customer.FirstName, Is.EqualTo("Delbert"));
        Assert.That(customer.LastName, Is.EqualTo("Traver"));
    }

    [Test]
    public void WhenPostCustomer_CustomerIsReturned()
    {
        var mockRepo = new Mock<ICustomersRepository>();
        mockRepo.Setup(repo => repo.Create(It.IsAny<CustomerAddEdit>()))
            .Returns(new Customer() { Id = 1, FirstName = "Delbert", LastName = "Traver" });
        var controller = new CustomersController(_mockLogger.Object, mockRepo.Object);

        var result = controller.Post(new CustomerAddEdit());

        var createdAtResult = result.Result as CreatedAtActionResult;
        Assert.That(createdAtResult, Is.Not.Null, "Expected CreatedAtActionResult but got null");
        var customer = createdAtResult.Value as Customer;
        Assert.That(customer, Is.Not.Null, "Expected Customer object but got null");
        Assert.That(customer.Id, Is.EqualTo(1));
        Assert.That(customer.FirstName, Is.EqualTo("Delbert"));
        Assert.That(customer.LastName, Is.EqualTo("Traver"));
    }

    [Test]
    public void WhenPutCustomerWithWrongId_NotFoundIsReturned()
    {
        var mockRepo = new Mock<ICustomersRepository>();
        mockRepo.Setup(repo => repo.Update(1, new CustomerAddEdit())).Returns((Customer?)null);
        var controller = new CustomersController(_mockLogger.Object, mockRepo.Object);

        var result = controller.Put(1, new CustomerAddEdit());

        Assert.That(result.Result is NotFoundObjectResult);
    }

    [Test]
    public void WhenPutCustomer_CustomerIsReturned()
    {
        var mockRepo = new Mock<ICustomersRepository>();
        mockRepo.Setup(repo => repo.Update(1, It.IsAny<CustomerAddEdit>()))
            .Returns(new Customer() { Id = 1, FirstName = "Delbert", LastName = "Traver" });
        var controller = new CustomersController(_mockLogger.Object, mockRepo.Object);

        var result = controller.Put(1, new CustomerAddEdit());

        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null, "Expected OkObjectResult but got null");
        var customer = okResult.Value as Customer;
        Assert.That(customer, Is.Not.Null, "Expected Customer object but got null");
        Assert.That(customer.Id, Is.EqualTo(1));
        Assert.That(customer.FirstName, Is.EqualTo("Delbert"));
        Assert.That(customer.LastName, Is.EqualTo("Traver"));
    }

    [Test]
    public void WhenDeleteCustomerWithWrongId_NotFoundIsReturned()
    {
        var mockRepo = new Mock<ICustomersRepository>();
        mockRepo.Setup(repo => repo.Delete(1)).Returns(false);
        var controller = new CustomersController(_mockLogger.Object, mockRepo.Object);

        var result = controller.Delete(1);

        Assert.That(result is NotFoundObjectResult);
    }

    [Test]
    public void WhenDeleteCustomer_NoContentIsReturned()
    {
        var mockRepo = new Mock<ICustomersRepository>();
        mockRepo.Setup(repo => repo.Delete(1)).Returns(true);
        var controller = new CustomersController(_mockLogger.Object, mockRepo.Object);

        var result = controller.Delete(1);

        Assert.That(result is NoContentResult);
    }

    private List<Customer> GetSampleCustomers()
    {
        return new List<Customer>()
        {
            new Customer() { Id = 1, FirstName = "Solomon", LastName = "Bleeker"},
            new Customer() { Id = 2, FirstName = "Delbert", LastName = "Traver"},
            new Customer() { Id = 3, FirstName = "Martin", LastName = "Farwell"}
        };
    }
}
