namespace Employees.Domain.Dtos;

public class LoginRequestDto
{
    public string DocumentNumber { get; set; }
    public string Password { get; set; }
}