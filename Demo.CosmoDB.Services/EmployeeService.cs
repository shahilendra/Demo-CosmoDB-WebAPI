using Demo.CosmoDB.Models;
using Demo.CosmoDB.Repository.Abstraction;
using Demo.CosmoDB.Services.Abstraction;
using Microsoft.Extensions.Logging;

namespace Demo.CosmoDB.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;
        private readonly ICacheService<Employees> _cacheService;
        public EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService> logger, ICacheService<Employees> cacheService)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        }

        public Task AddAsync(Employees item)
        {
            try
            {
                _logger.LogInformation($"{nameof(EmployeeService)}.{nameof(AddAsync)} started!");
                return _employeeRepository.AddAsync(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EmployeeService)}.{nameof(AddAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                _logger.LogInformation($"{nameof(EmployeeService)}.{nameof(DeleteAsync)} started!");
                await _cacheService.RemoveAsync(id, async () =>
                {
                    await _employeeRepository.DeleteAsync(id);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EmployeeService)}.{nameof(DeleteAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }

        public async Task<Employees> GetAsync(string id)
        {
            try
            {
                _logger.LogInformation($"{nameof(EmployeeService)}.{nameof(GetAsync)} started!");
                return await _cacheService.GetOrAddAsync(id, async () =>
                {
                    return await _employeeRepository.GetAsync(id);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EmployeeService)}.{nameof(GetAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }

        public async Task UpdateAsync(string id, Employees item)
        {
            try
            {
                _logger.LogInformation($"{nameof(EmployeeService)}.{nameof(UpdateAsync)} started!");
                await _cacheService.RemoveAsync(id, async () =>
                {
                    await _employeeRepository.UpdateAsync(id, item);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EmployeeService)}.{nameof(UpdateAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }
        public async Task<List<Employees>> GetAsync()
        {
            try
            {
                _logger.LogInformation($"{nameof(EmployeeService)}.{nameof(GetAsync)} started!");
                return await _employeeRepository.GetAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EmployeeService)}.{nameof(GetAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }
    }
}