using EMS_MiniProject.DTOs;

namespace EMS_MiniProject.Services
{
    public interface IAuthService
    {
        // 🔹 User Register
        Task<string> RegisterAsync(RegisterRequestDto request);
        Task<LoginResponseDto?> AuthenticateAsync(LoginRequestDto request);
    }
}
