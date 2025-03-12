namespace Employees.Domain.Interfaces;

public interface IAuthenticationService
{
    Task<string> AuthenticateAsync(string documentNumber, string password);
}