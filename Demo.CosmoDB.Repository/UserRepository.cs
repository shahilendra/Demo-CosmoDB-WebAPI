using Demo.CosmoDB.Models;
using Demo.CosmoDB.Repository.Abstraction;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.CosmoDB.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly CosmosDbClient<User> _cosmosDbClient;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(CosmosDbClient<User> cosmosDbClient, ILogger<UserRepository> logger)
        {
            _cosmosDbClient = cosmosDbClient ?? throw new ArgumentNullException(nameof(cosmosDbClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task AddAsync(User item)
        {
            try
            {
                _logger.LogInformation($"{nameof(UserRepository)}.{nameof(AddAsync)} started!");
                return _cosmosDbClient.AddAsync(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(UserRepository)}.{nameof(AddAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }

        public Task DeleteAsync(string id)
        {
            try
            {
                _logger.LogInformation($"{nameof(UserRepository)}.{nameof(DeleteAsync)} started!");
                return _cosmosDbClient.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(UserRepository)}.{nameof(DeleteAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }

        public Task<User> GetAsync(string id)
        {
            try
            {
                _logger.LogInformation($"{nameof(UserRepository)}.{nameof(GetAsync)} started!");
                return _cosmosDbClient.GetAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(UserRepository)}.{nameof(GetAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }

        public Task UpdateAsync(string id, User item)
        {
            try
            {
                _logger.LogInformation($"{nameof(UserRepository)}.{nameof(UpdateAsync)} started!");
                return _cosmosDbClient.UpdateAsync(id, item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(UserRepository)}.{nameof(UpdateAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }
        public async Task<List<User>> GetAsync()
        {
            try
            {
                _logger.LogInformation($"{nameof(UserRepository)}.{nameof(GetAsync)} started!");
                return await _cosmosDbClient.GetAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(UserRepository)}.{nameof(GetAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }
    }
}
