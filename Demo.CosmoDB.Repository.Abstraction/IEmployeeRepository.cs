using Demo.CosmoDB.Models;

namespace Demo.CosmoDB.Repository.Abstraction
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employees item);
        Task DeleteAsync(string id);
        Task<Employees> GetAsync(string id);
        Task UpdateAsync(string id, Employees item);
        Task<List<Employees>> GetAsync();

    }
}