using System.Text.Json.Serialization;
using Employees.Domain.Entities;
using Employees.Domain.Enums;

namespace Employees.Domain.Dtos;

public class EmployeeDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string LastName { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("documentNumber")]
    public string DocumentNumber { get; set; }

    [JsonPropertyName("birthDate")]
    public DateTime BirthDate { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    [JsonPropertyName("phones")]
    public List<string> Phones { get; set; }

    [JsonPropertyName("managerId")]
    public string? ManagerId { get; set; }

    [JsonPropertyName("role")]
    public int Role { get; set; }
    
    public static EmployeeDto FromEmployee(Employee employee)
    {
        return new EmployeeDto
        {
            Id = employee.Id.ToString(),
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            DocumentNumber = employee.DocumentNumber,
            BirthDate = employee.BirthDate,
            Password = string.Empty, // Password should not be exposed
            Phones = employee.Phones,
            ManagerId = employee.ManagerId?.ToString(),
            Role = (int)employee.Role
        };
    }
    
    public static IEnumerable<EmployeeDto> FromEmployeeList(IEnumerable<Employee> employees)
    {
        return employees.Select(FromEmployee).ToList();
    }
}