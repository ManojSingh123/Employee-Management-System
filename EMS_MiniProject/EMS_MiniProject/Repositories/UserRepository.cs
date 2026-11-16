using EMS_MiniProject.DataContext;
using EMS_MiniProject.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS_MiniProject.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;   
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserAsync(string username)
        {

            return await _context.Users
                                 .Include(u => u.Role)   // 🔥 MUST ADD THIS
                                 .FirstOrDefaultAsync(u => u.Username == username);

            //return await _context.Users
            //                     .FirstOrDefaultAsync(u => u.Username == username);   
        }
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllEmployeesAsync()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Role.RoleName == "Employee")
                .ToListAsync();
        }

        // GET ALL
        //public async Task<List<Employee>> GetAllAsync()
        //{
        //    return await _context.Employees.ToListAsync();
        //}

        //// GET BY ID
        //public async Task<Employee?> GetByIdAsync(int id)
        //{
        //    return await _context.Employees.FindAsync(id);
        //}

        //// INSERT
        //public async Task AddAsync(Employee employee)
        //{
        //    await _context.Employees.AddAsync(employee);
        //    await _context.SaveChangesAsync();
        //}

        //// UPDATE
        //public async Task UpdateAsync(Employee employee)
        //{
        //    _context.Employees.Update(employee);
        //    await _context.SaveChangesAsync();
        //}

        //// DELETE
        //public async Task DeleteAsync(int id)
        //{
        //    var emp = await _context.Employees.FindAsync(id);
        //    if (emp != null)
        //    {
        //        _context.Employees.Remove(emp);
        //        await _context.SaveChangesAsync();
        //    }
        //}
    }
}
