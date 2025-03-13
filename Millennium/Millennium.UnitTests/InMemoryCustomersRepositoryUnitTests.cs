using Millennium.Api.Models;
using Millennium.Api.Repositories;

namespace Millennium.UnitTests.Repositories;

internal class InMemoryCustomersRepositoryUnitTests
{
    [Test]
    public void WhenCreateCustomer_IdIsAssigned()
    {
        var repository = new InMemoryCustomersRepository();

        var customer = repository.Create(new CustomerAddEdit());

        Assert.That(customer.Id > 0);
    }

    [Test]
    public void WhenCreateCustomer_NewCustomerIsReturned()
    {
        var repository = new InMemoryCustomersRepository();

        var customer = repository.Create(new CustomerAddEdit() { FirstName = "Test Name", LastName = "Test Surname" });

        Assert.That(customer?.FirstName == "Test Name");
        Assert.That(customer?.LastName == "Test Surname");
        Assert.That(customer?.Id > 0);
    }

    [Test]
    public void WhenGetAllCustomers_AllCustomersAreReturned()
    {
        var repository = GetRepositoryWithSampleData();

        var customers = repository.GetAll();

        Assert.That(customers?.Count == 3);
    }

    [Test]
    public void WhenCreateCustomer_ExactlyOneCustomerIsAdded()
    {
        var repository = new InMemoryCustomersRepository();
        var beforeCount = repository.GetAll().Count;
        repository.Create(new CustomerAddEdit() { FirstName = "Test Name", LastName = "Test Surname" });

        var afterCount = repository.GetAll().Count;

        Assert.That(afterCount - beforeCount == 1);
    }

    [Test]
    public void WhenGetByIdCustomer_CustomerIsReturned()
    {
        var repository = GetRepositoryWithSampleData();

        var customer = repository.GetById(2);

        Assert.That(customer?.Id == 2);
        Assert.That(customer?.FirstName == "Delbert");
        Assert.That(customer?.LastName == "Traver");
    }

    [Test]
    public void WhenGetByIdCustomerWithWrongId_NullIsReturned()
    {
        var repository = GetRepositoryWithSampleData();

        var customer = repository.GetById(4);

        Assert.That(customer == null);
    }

    [Test]
    public void WhenUpdateCustomer_CustomerIsUpdated()
    {
        var repository = GetRepositoryWithSampleData();

        var updated = repository.Update(3, new CustomerAddEdit() { FirstName = "New Name", LastName = "New Last" });

        Assert.That(updated?.Id == 3);
        Assert.That(updated?.FirstName == "New Name");
        Assert.That(updated?.LastName == "New Last");
    }

    [Test]
    public void WhenUpdateCustomerWithWrongId_NullIsReturned()
    {
        var repository = GetRepositoryWithSampleData();

        var updated = repository.Update(4, new CustomerAddEdit() { FirstName = "New Name", LastName = "New Last" });

        Assert.That(updated == null);
    }

    [Test]
    public void WhenDeleteCustomer_CustomerIsDeleted()
    {
        var repository = GetRepositoryWithSampleData();

        var customerBefore = repository.GetById(1);
        repository.Delete(1);
        var customerAfer = repository.GetById(1);

        Assert.That(customerBefore != null);
        Assert.That(customerAfer == null);
    }

    [Test]
    public void WhenDeleteCustomerWithWrongId_FalseIsReturned()
    {
        var repository = GetRepositoryWithSampleData();

        var ifDeleted = repository.Delete(4);

        Assert.That(ifDeleted == false);
    }

    private InMemoryCustomersRepository GetRepositoryWithSampleData()
    {
        var repository = new InMemoryCustomersRepository();
        repository.Create(new CustomerAddEdit() { FirstName = "Solomon", LastName = "Bleeker" });
        repository.Create(new CustomerAddEdit() { FirstName = "Delbert", LastName = "Traver" });
        repository.Create(new CustomerAddEdit() { FirstName = "Martin", LastName = "Farwell" });
        return repository;
    }
}
