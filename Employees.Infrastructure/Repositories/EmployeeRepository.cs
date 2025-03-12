using Employees.Domain.Entities;
using Employees.Domain.Interfaces;
using Employees.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Employees.Infrastructure.Repositories;

public class EmployeeRepository(AppDbContext context) : IEmployeeRepository
{
    public async Task<Employee?> GetByIdAsync(Guid id)
    {
        return await context.Employees.FindAsync(id);
    }

    public Task<Employee?> GetByDocumentNumberAsync(string documentNumber)
    {
        return context.Employees.FirstOrDefaultAsync(e => e.DocumentNumber == documentNumber);
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        return await context.Employees.ToListAsync();
    }

    public async Task<Guid> AddAsync(Employee employee)
    {
        await context.Employees.AddAsync(employee);
        await context.SaveChangesAsync();
        return employee.Id;
    }

    public async Task UpdateAsync(Employee employee)
    {
        context.Employees.Update(employee);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var employee = await GetByIdAsync(id);
        if (employee != null)
        {
            context.Employees.Remove(employee);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsByDocumentNumberAsync(string documentNumber)
    {
        return await context.Employees.AnyAsync(e => e.DocumentNumber == documentNumber);
    }
}