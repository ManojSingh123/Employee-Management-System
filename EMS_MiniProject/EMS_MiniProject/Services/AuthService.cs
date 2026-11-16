using EMS_MiniProject.DTOs;
using EMS_MiniProject.Models;
using EMS_MiniProject.Repositories;
using System.IdentityModel.Tokens.Jwt;

namespace EMS_MiniProject.Services
{
    public class AuthService:IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }


        // 🔹 REGISTER
        public async Task<string> RegisterAsync(RegisterRequestDto request)
        {
            var existingUser = await _userRepository.GetUserAsync(request.Username);

            if (existingUser != null)
                return "User already exists";

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                PasswordHash = hashedPassword,
                RoleId = request.RoleId
            };

            await _userRepository.AddUserAsync(user);

            return "User Registered Successfully";
        }

        public async Task<LoginResponseDto?> AuthenticateAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetUserAsync(request.Username);

            if (user == null)
                return null;

            bool isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isValid)
                return null;


            Console.WriteLine(_config["Jwt:Key"]);
            Console.WriteLine(_config["Jwt:Issuer"]);
            Console.WriteLine(_config["Jwt:ExpireMinutes"]);

            var token = JwtHelper.GenerateJwtToken(
                user,
                _config["Jwt:Key"],
                _config["Jwt:Issuer"],
                _config["Jwt:ExpireMinutes"]
            );

            return new LoginResponseDto
            {
                Token = token,
                Role = user.Role?.RoleName ?? "User"
            };
        }
    }
}
