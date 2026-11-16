using EMS_MiniProject.DTOs;
using EMS_MiniProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace EMS_MiniProject.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        //{
        //    var result = await _authService.AuthenticateAsync(request);
        //    if (result == null)
        //        return Unauthorized("Invalid username or password");

        //    return Ok(result);
        //}

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);

            if (result == "User already exists")
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var response = await _authService.AuthenticateAsync(request);

            if (response == null)
                return Unauthorized("Invalid username or password");

            return Ok(response);
        }
    }
}
