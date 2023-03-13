using Demo.CosmoDB.Models;
using Demo.CosmoDB.Repository.Abstraction;
using Microsoft.Extensions.Logging;

namespace Demo.CosmoDB.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly CosmosDbClient<Employees> _cosmosDbClient;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(CosmosDbClient<Employees> cosmosDbClient, ILogger<EmployeeRepository> logger)
        {
            _cosmosDbClient = cosmosDbClient ?? throw new ArgumentNullException(nameof(cosmosDbClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task AddAsync(Employees item)
        {
            try
            {
                _logger.LogInformation($"{nameof(EmployeeRepository)}.{nameof(AddAsync)} started!");
                return _cosmosDbClient.AddAsync(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EmployeeRepository)}.{nameof(AddAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }

        public Task DeleteAsync(string id)
        {
            try
            {
                _logger.LogInformation($"{nameof(EmployeeRepository)}.{nameof(DeleteAsync)} started!");
                return _cosmosDbClient.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EmployeeRepository)}.{nameof(DeleteAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }

        public Task<Employees> GetAsync(string id)
        {
            try
            {
                _logger.LogInformation($"{nameof(EmployeeRepository)}.{nameof(GetAsync)} started!");
                return _cosmosDbClient.GetAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EmployeeRepository)}.{nameof(GetAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }

        public Task UpdateAsync(string id, Employees item)
        {
            try
            {
                _logger.LogInformation($"{nameof(EmployeeRepository)}.{nameof(UpdateAsync)} started!");
                return _cosmosDbClient.UpdateAsync(id, item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EmployeeRepository)}.{nameof(UpdateAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }
        public async Task<List<Employees>> GetAsync()
        {
            try
            {
                _logger.LogInformation($"{nameof(EmployeeRepository)}.{nameof(GetAsync)} started!");
                return await _cosmosDbClient.GetAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EmployeeRepository)}.{nameof(GetAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }
    }
}