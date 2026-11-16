using EMS_MiniProject.DataContext;
using EMS_MiniProject.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS_MiniProject.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;
        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        // 🔹 Get All Employees
        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        // 🔹 Get Employee By Id
        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        // 🔹 Add Employee
        public async Task<Employee> AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        // 🔹 Update Employee
        public async Task<Employee?> UpdateAsync(Employee employee)
        {
            var existingEmployee = await _context.Employees.FindAsync(employee.Id);

            if (existingEmployee == null)
                return null;

            existingEmployee.Name = employee.Name;
            existingEmployee.Email = employee.Email;
            existingEmployee.Salary = employee.Salary;

            await _context.SaveChangesAsync();
            return existingEmployee;
        }

        // 🔹 Delete Employee
        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
