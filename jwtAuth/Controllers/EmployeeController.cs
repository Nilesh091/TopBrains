using jwtAuth.Models;
using jwtAuth.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jwtAuth.Controllers
{
    [Authorize]
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        // 🔁 Dependency Injection
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // ✅ GET: api/employees
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var employees = _employeeRepository.GetAllEmployees();
            return Ok(employees);
        }

        // ✅ GET: api/employees/1
        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            var employee = _employeeRepository.GetEmployeeById(id);

            if (employee == null)
                return NotFound(new { message = "Employee not found" });

            return Ok(employee);
        }

        // ✅ POST: api/employees
        [HttpPost]
        public IActionResult AddEmployee([FromBody] Employee employee)
        {
            if (employee == null)
                return BadRequest(new { message = "Invalid employee data" });

            _employeeRepository.AddEmployee(employee);

            return CreatedAtAction(
                nameof(GetEmployeeById),
                new { id = employee.Id },
                employee
            );
        }

        // ✅ PUT: api/employees/1
        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] Employee employee)
        {
            if (employee == null || id != employee.Id)
                return BadRequest(new { message = "Invalid request data" });

            var existingEmployee = _employeeRepository.GetEmployeeById(id);

            if (existingEmployee == null)
                return NotFound(new { message = "Employee not found" });

            _employeeRepository.UpdateEmployee(employee);

            return Ok(new { message = "Employee updated successfully" });
        }

        // ✅ DELETE: api/employees/1
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var existingEmployee = _employeeRepository.GetEmployeeById(id);

            if (existingEmployee == null)
                return NotFound(new { message = "Employee not found" });

            _employeeRepository.DeleteEmployee(id);

            return Ok(new { message = "Employee deleted successfully" });
        }
    }
}
