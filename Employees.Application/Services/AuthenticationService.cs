using Employees.Domain.Interfaces;
using Employees.Infrastructure.Security;

namespace Employees.Application.Services;

public class AuthenticationService(IEmployeeRepository employeeRepository, IPasswordHasher passwordHasher, IJwtService jwtService) : IAuthenticationService
{
    public async Task<string> AuthenticateAsync(string documentNumber, string password)
    {
        
        var employee = (await employeeRepository.GetByDocumentNumberAsync(documentNumber));
        if (employee == null) return "";

        return passwordHasher.VerifyPassword(password, employee.PasswordHash) ? jwtService.GenerateToken(employee) : "";
    }
}