using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.CosmoDB.Services.Abstraction
{
    public interface ICacheService<T>
    {
        T GetOrAdd(string key, Func<T> retriever);
        Task<T> GetOrAddAsync(string key, Func<Task<T>> retriever);
        Task RemoveAsync(string key, Func<Task> retriever);
    }
}
