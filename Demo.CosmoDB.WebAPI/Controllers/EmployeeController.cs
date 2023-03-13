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
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation($"{nameof(EmployeeController)}.{nameof(Get)} started!");
                return Ok(await _employeeService.GetAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EmployeeController)}.{nameof(Get)} getting an error: {ex.Message}!");
                throw;
            }

        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                _logger.LogInformation($"{nameof(EmployeeController)}.{nameof(Get)}/{id} started!");
                return Ok(await _employeeService.GetAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EmployeeController)}.{nameof(Get)}/{id} getting an error: {ex.Message}!");
                throw;
            }
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employees employee)
        {
            try
            {
                _logger.LogInformation($"{nameof(EmployeeController)}.{nameof(Post)} started!");
                if (employee.id == null)
                {
                    return BadRequest("Employee was empty!");
                }
                var emp = _employeeService.GetAsync(employee?.id?.ToString());
                if(emp != null)
                {
                    return BadRequest("Employee alredy exist!");
                }
                await _employeeService.AddAsync(employee);
                return Ok(new { message = "Employee Added successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EmployeeController)}.{nameof(Post)} getting an error: {ex.Message}!");
                throw;
            }
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Employees employee)
        {
            try
            {
                _logger.LogInformation($"{nameof(EmployeeController)}.{nameof(Put)} started!");
                if (id == null)
                {
                    return BadRequest("Employee was empty!");
                }
                var emp = _employeeService.GetAsync(id);
                if (emp == null)
                {
                    return BadRequest("Employee is not exist!");
                }
                await _employeeService.UpdateAsync(id, employee);
                return Ok(new { message = "Employee updated successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EmployeeController)}.{nameof(Put)} getting an error: {ex.Message}!");
                throw;
            }
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                _logger.LogInformation($"{nameof(EmployeeController)}.{nameof(Delete)} started!");
                var emp = _employeeService.GetAsync(id);
                if (emp == null)
                {
                    return BadRequest("Employee is not exist!");
                }
                await _employeeService.DeleteAsync(id);
                return Ok(new { message = "Employee deleted successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(EmployeeController)}.{nameof(Delete)} getting an error: {ex.Message}!");
                throw;
            }
        }
    }
}
