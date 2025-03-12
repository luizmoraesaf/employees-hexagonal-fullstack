using System.Text.Json.Serialization;

namespace Employees.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EmployeeRole
{
    Employee = 1,   // Funcionário comum
    Leader = 2,     // Líder
    Director = 3    // Diretor
}