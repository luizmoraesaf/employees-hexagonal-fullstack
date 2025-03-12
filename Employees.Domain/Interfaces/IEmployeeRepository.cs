using Employees.Domain.Entities;

namespace Employees.Domain.Interfaces;

public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(Guid id);
    Task<Employee?> GetByDocumentNumberAsync(string documentNumber);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<Guid> AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsByDocumentNumberAsync(string documentNumber);
}