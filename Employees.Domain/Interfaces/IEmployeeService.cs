using Employees.Domain.Dtos;
using Employees.Domain.Entities;

namespace Employees.Domain.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeDto?> GetByIdAsync(Guid id);
    Task<EmployeeDto?> GetByDocumentNumberAsync(string documentNumber);
    Task<IEnumerable<EmployeeDto>> GetAllAsync();
    Task<Guid> CreateAsync(EmployeeDto employee, Guid createdById);
    Task<bool> UpdateAsync(EmployeeDto employee, Guid createdById);
    Task<bool> DeleteAsync(Guid id);
}