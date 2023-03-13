using Demo.CosmoDB.Models;

namespace Demo.CosmoDB.Services.Abstraction
{
    public interface IEmployeeService
    {
        Task AddAsync(Employees item);
        Task DeleteAsync(string id);
        Task<Employees> GetAsync(string id);
        Task UpdateAsync(string id, Employees item);
        Task<List<Employees>> GetAsync();
    }
}