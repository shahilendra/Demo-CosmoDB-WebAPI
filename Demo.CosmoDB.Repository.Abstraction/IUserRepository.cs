using Demo.CosmoDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.CosmoDB.Repository.Abstraction
{
    public interface IUserRepository
    {
        Task AddAsync(User item);
        Task DeleteAsync(string id);
        Task<User> GetAsync(string id);
        Task UpdateAsync(string id, User item);
        Task<List<User>> GetAsync();
    }
}
