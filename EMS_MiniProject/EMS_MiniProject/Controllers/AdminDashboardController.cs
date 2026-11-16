using EMS_MiniProject.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMS_MiniProject.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {

        private readonly IUserRepository _userRepo;

        public AdminDashboardController(IUserRepository userRepo) => _userRepo = userRepo;

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var employees = await _userRepo.GetAllEmployeesAsync();

            return Ok(new
            {
                message = "Admin Dashboard",
                totalEmployees = employees.Count(),
                employees = employees.Select(e => new { e.Id, e.Username })
            });
        }
    }
}
