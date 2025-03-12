using Employees.Domain.Dtos;
using Employees.Domain.Entities;
using Employees.Domain.Enums;
using Employees.Domain.Interfaces;

namespace Employees.Application.Services;

public class EmployeeService(IEmployeeRepository employeeRepository, IPasswordHasher passwordHasher) : IEmployeeService
{
    public async Task<EmployeeDto?> GetByIdAsync(Guid id)
    {
        var employee = await employeeRepository.GetByIdAsync(id);
        
        return employee == null ? null : EmployeeDto.FromEmployee(employee);
    }

    public async Task<EmployeeDto?> GetByDocumentNumberAsync(string documentNumber)
    {
        var employee = await employeeRepository.GetByDocumentNumberAsync(documentNumber);
        
        return employee == null ? null : EmployeeDto.FromEmployee(employee);
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employeeList = await employeeRepository.GetAllAsync();
        
        return EmployeeDto.FromEmployeeList(employeeList);
    }
    
    public async Task<bool> IsEmployeeValid(EmployeeDto employee, Guid createdById)
    {
        var creator = await employeeRepository.GetByIdAsync(createdById);
        if (creator == null)
            throw new UnauthorizedAccessException("User unauthorized.");
        
        if (employee.Role > (int)creator.Role)
            throw new UnauthorizedAccessException("You can't create a employee with a role higher than yours.");
        
        if (employee.BirthDate > DateTime.UtcNow.AddYears(-18))
            throw new Exception("The employee must be at least 18 years old.");

        return true;
    }

    public async Task<Guid> CreateAsync(EmployeeDto employee, Guid createdById)
    {
        if (!await this.IsEmployeeValid(employee, createdById))
            throw new ApplicationException("Error creating the user.");
        if (await employeeRepository.ExistsByDocumentNumberAsync(employee.DocumentNumber))
            throw new Exception("Already exists an employee with this document number.");
        
        var passwordHash = passwordHasher.HashPassword(employee.Password);
            
        return await employeeRepository.AddAsync(new Employee(employee.FirstName, employee.LastName, employee.Email, employee.DocumentNumber, employee.BirthDate, employee.Phones,  passwordHash, (EmployeeRole)employee.Role, employee.ManagerId != "" ? Guid.Parse(employee.ManagerId ?? string.Empty) : null));

    }

    public async Task<bool> UpdateAsync(EmployeeDto employee, Guid createdById)
    {
        if (!await this.IsEmployeeValid(employee, createdById))
            throw new ApplicationException("Error updating the user.");
        
        var passwordHash = passwordHasher.HashPassword(employee.Password);

        var updatedEmployee = new Employee(employee.FirstName, employee.LastName, employee.Email,
            employee.DocumentNumber, employee.BirthDate, employee.Phones, passwordHash, (EmployeeRole)employee.Role,
            employee.ManagerId != "" ? Guid.Parse(employee.ManagerId ?? string.Empty) : null);
        updatedEmployee.OverrideId(Guid.Parse(employee.Id!));
        
        await employeeRepository.UpdateAsync(updatedEmployee);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        await employeeRepository.DeleteAsync(id);
        return true;
    }
}