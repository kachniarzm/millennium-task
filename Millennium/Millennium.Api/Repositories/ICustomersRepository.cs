using Millennium.Api.Models;

namespace Millennium.Api.Repositories;

public interface ICustomersRepository
{
    List<Customer> GetAll();
    Customer? GetById(int id);
    Customer Create(CustomerAddEdit customer);
    Customer? Update(int id, CustomerAddEdit customer);
    bool Delete(int id);
}
