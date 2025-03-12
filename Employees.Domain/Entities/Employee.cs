using Employees.Domain.Enums;
using Employees.Domain.Exceptions;

namespace Employees.Domain.Entities;

public class Employee
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string DocumentNumber { get; private set; }
    public DateTime BirthDate { get; private set; }
    public string PasswordHash { get; private set; }
    public List<string> Phones { get; private set; } = new();
    public Guid? ManagerId { get; private set; }
    public EmployeeRole Role { get; private set; }

    public Employee(string firstName, string lastName, string email, string documentNumber, DateTime birthDate, List<string> phones, string passwordHash, EmployeeRole role, Guid? managerId)
    {
        if (birthDate > DateTime.UtcNow.AddYears(-18))
            throw new DomainException("Employee must be at least 18 years old.");

        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        DocumentNumber = documentNumber;
        BirthDate = birthDate;
        PasswordHash = passwordHash;
        Role = role;
        Phones = phones;
        ManagerId = managerId;
    }

    public void AddPhone(string phone)
    {
        Phones.Add(phone);
    }

    public void SetManager(Guid managerId)
    {
        ManagerId = managerId;
    }
    
    public void OverrideId(Guid id)
    {
        Id = id;
    }
}