using EMS_MiniProject.Models;

namespace EMS_MiniProject.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string username);
        Task AddUserAsync(User user);
        Task<List<User>> GetAllEmployeesAsync();
        
    }
}
