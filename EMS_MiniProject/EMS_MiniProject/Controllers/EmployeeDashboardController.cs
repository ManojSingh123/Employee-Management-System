using EMS_MiniProject.Models;
using EMS_MiniProject.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS_MiniProject.Controllers
{
    [ApiController]
    [Route("api/employee")]
    [Authorize]
    public class EmployeeDashboardController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;


        public EmployeeDashboardController(
            IUserRepository userRepository,
            IEmployeeRepository employeeRepository)
        {
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
        }

        // ================================
        // 🔹 DASHBOARD (Admin + Employee)
        // ================================
        [HttpGet("dashboard")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                var username = User.Identity?.Name;

                if (string.IsNullOrEmpty(username))
                    return Unauthorized("Invalid token");

                var user = await _userRepository.GetUserAsync(username);

                if (user == null)
                    return NotFound("User not found");

                return Ok(new
                {
                    user.Id,
                    user.Username,
                    Role = user.Role?.RoleName
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        // ================================
        // 🔹 GET ALL (Admin/Employee Only)
        // ================================
        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var employees = await _employeeRepository.GetAllAsync();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        // ================================
        // 🔹 GET BY ID (Admin + Employee)
        // ================================
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var employee = await _employeeRepository.GetByIdAsync(id);

                if (employee == null)
                    return NotFound("Employee not found");

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        // ================================
        // 🔹 CREATE (Admin Only)
        // ================================
        [HttpPost]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Create([FromBody] Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var created = await _employeeRepository.AddAsync(employee);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        // ================================
        // 🔹 UPDATE (Admin Only)
        // ================================
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Employee employee)
        {
            try
            {
                if (id != employee.Id)
                    return BadRequest("ID mismatch");

                var updated = await _employeeRepository.UpdateAsync(employee);

                if (updated == null)
                    return NotFound("Employee not found");

                return Ok(updated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        // ================================
        // 🔹 DELETE (Admin Only)
        // ================================
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _employeeRepository.DeleteAsync(id);

                if (!deleted)
                    return NotFound("Employee not found");

                return Ok("Deleted Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }
    }
}