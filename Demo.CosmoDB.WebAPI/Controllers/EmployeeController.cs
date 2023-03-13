using Demo.CosmoDB.Models;
using Demo.CosmoDB.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Demo.CosmoDB.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;
        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/<EmployeeController>
        [HttpGet]
        public async Task<List<Employees>> Get()
        {
            try
            {
                _logger.LogInformation($"{nameof(EmployeeController)}.{nameof(Get)} started!");
                return await _employeeService.GetAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EmployeeController)}.{nameof(Get)} getting an error: {ex.Message}!");
                throw;
            }

        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public async Task<Employees> Get(string id)
        {
            try
            {
                _logger.LogInformation($"{nameof(EmployeeController)}.{nameof(Get)}/{id} started!");
                return await _employeeService.GetAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EmployeeController)}.{nameof(Get)}/{id} getting an error: {ex.Message}!");
                throw;
            }
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public async Task Post([FromBody] Employees employee)
        {
            try
            {
                _logger.LogInformation($"{nameof(EmployeeController)}.{nameof(Post)} started!");
                await _employeeService.AddAsync(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EmployeeController)}.{nameof(Post)} getting an error: {ex.Message}!");
                throw;
            }
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public async Task Put(string id, [FromBody] Employees employee)
        {
            try
            {
                _logger.LogInformation($"{nameof(EmployeeController)}.{nameof(Put)} started!");
                await _employeeService.UpdateAsync(id, employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EmployeeController)}.{nameof(Put)} getting an error: {ex.Message}!");
                throw;
            }
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            try
            {
                _logger.LogInformation($"{nameof(EmployeeController)}.{nameof(Delete)} started!");
                await _employeeService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EmployeeController)}.{nameof(Delete)} getting an error: {ex.Message}!");
                throw;
            }
        }
    }
}
