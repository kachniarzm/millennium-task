using Millennium.Api.Models;

namespace Millennium.Api.Repositories;

public class InMemoryCustomersRepository : ICustomersRepository
{
    private readonly List<Customer> _customers = new();
    // Use to mock incremental id, starting from 1
    private int index = 1;

    public InMemoryCustomersRepository() { }

    public Customer Create(CustomerAddEdit customer)
    {
        var newCustomer = new Customer()
        {
            Id = index++,
            FirstName = customer.FirstName,
            LastName = customer.LastName
        };

        _customers.Add(newCustomer);

        return newCustomer;
    }

    public bool Delete(int id)
    {
        return _customers.RemoveAll(x => x.Id == id) > 0;
    }

    public List<Customer> GetAll()
    {
        return _customers;
    }

    public Customer? GetById(int id)
    {
        return _customers.FirstOrDefault(x => x.Id == id);
    }

    public Customer? Update(int id, CustomerAddEdit customer)
    {
        var exisintg = GetById(id);

        if (exisintg == null) return null;

        exisintg.FirstName = customer.FirstName;
        exisintg.LastName = customer.LastName;

        return exisintg;
    }
}
