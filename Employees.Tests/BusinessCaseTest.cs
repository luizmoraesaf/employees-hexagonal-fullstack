using Employees.Application.Services;
using Employees.Domain.Dtos;
using Employees.Domain.Entities;
using Employees.Domain.Enums;
using Employees.Domain.Interfaces;
using Employees.WebAPI.Validators;
using FluentValidation;
using Moq;

namespace Employees.Tests;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IEmployeeService> _employeeServiceMock;
    private readonly EmployeeService _employeeService;
    private readonly IValidator<EmployeeDto> _validator;

    public EmployeeServiceTests()
    {
        _employeeRepositoryMock = new Mock<IEmployeeRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _employeeService = new EmployeeService(_employeeRepositoryMock.Object, _passwordHasherMock.Object);
        _employeeServiceMock = new Mock<IEmployeeService>();
        _validator = new EmployeeValidator();
    }

    [Fact]
    public async Task CreateEmployee_ShouldReturnValidationErrors_WhenInvalidData()
    {
        // Arrange
        var invalidEmployee = new EmployeeDto
        {
            FirstName = "",
            LastName = "",
            Email = "invalid-email",
            DocumentNumber = "",
            BirthDate = DateTime.Now.AddYears(1),
            Role = 0,
            Phones = new List<string>(),
            Password = "weak"
        };

        // Act
        var validationResult = await _validator.ValidateAsync(invalidEmployee);

        // Assert
        Assert.False(validationResult.IsValid);
        Assert.Contains(validationResult.Errors, e => e.PropertyName == "FirstName");
        Assert.Contains(validationResult.Errors, e => e.PropertyName == "LastName");
        Assert.Contains(validationResult.Errors, e => e.PropertyName == "Email");
        Assert.Contains(validationResult.Errors, e => e.PropertyName == "DocumentNumber");
        Assert.Contains(validationResult.Errors, e => e.PropertyName == "BirthDate");
        Assert.Contains(validationResult.Errors, e => e.PropertyName == "Role");
        Assert.Contains(validationResult.Errors, e => e.PropertyName == "Phones");
        Assert.Contains(validationResult.Errors, e => e.PropertyName == "Password");
    }

    [Fact]
    public async Task CreateEmployee_ShouldPassValidation_WhenValidData()
    {
        // Arrange
        var validEmployee = new EmployeeDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            DocumentNumber = "123456789",
            BirthDate = DateTime.Now.AddYears(-30),
            Role = 1,
            Phones = new List<string> { "123-456-7890" },
            Password = "StrongP@ssw0rd"
        };

        // Act
        var validationResult = await _validator.ValidateAsync(validEmployee);

        // Assert
        Assert.True(validationResult.IsValid);
    }

    [Fact]
    public async Task CreateEmployee_ShouldThrowException_WhenServiceFails()
    {
        // Arrange
        var validEmployee = new EmployeeDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            DocumentNumber = "123456789",
            BirthDate = DateTime.Now.AddYears(-30),
            Role = 1,
            Phones = new List<string> { "123-456-7890" },
            Password = "StrongP@ssw0rd"
        };

        _employeeServiceMock.Setup(service => service.CreateAsync(It.IsAny<EmployeeDto>(), It.IsAny<Guid>()))
            .ThrowsAsync(new Exception("Service error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _employeeServiceMock.Object.CreateAsync(validEmployee, Guid.NewGuid()));
        Assert.Equal("Service error", exception.Message);
    }
    
    [Fact]
    public async Task IsEmployeeValid_ShouldThrowUnauthorizedAccessException_WhenCreatorNotFound()
    {
        // Arrange
        var employee = new EmployeeDto();
        var createdById = Guid.NewGuid();

        _employeeRepositoryMock.Setup(repo => repo.GetByIdAsync(createdById))
            .ReturnsAsync((Employee?)null);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _employeeService.IsEmployeeValid(employee, createdById));
    }

    [Fact]
    public async Task IsEmployeeValid_ShouldThrowUnauthorizedAccessException_WhenEmployeeRoleHigherThanCreator()
    {
        // Arrange
        var employee = new EmployeeDto { Role = (int)EmployeeRole.Leader };
        var createdById = Guid.NewGuid();
        var creator = new Employee("firstName", "lastName",  "email", "documentNumber", DateTime.UtcNow.AddYears(-20), new List<string>(), "password", EmployeeRole.Employee, Guid.Empty);

        _employeeRepositoryMock.Setup(repo => repo.GetByIdAsync(createdById))
            .ReturnsAsync(creator);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _employeeService.IsEmployeeValid(employee, createdById));
    }

    [Fact]
    public async Task IsEmployeeValid_ShouldThrowException_WhenEmployeeIsUnderage()
    {
        // Arrange
        var employee = new EmployeeDto { BirthDate = DateTime.UtcNow.AddYears(-17) };
        var createdById = Guid.NewGuid();
        var creator = new Employee("firstName", "lastName",  "email", "documentNumber", DateTime.UtcNow.AddYears(-20), new List<string>(), "password", EmployeeRole.Director, Guid.Empty);

        _employeeRepositoryMock.Setup(repo => repo.GetByIdAsync(createdById))
            .ReturnsAsync(creator);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _employeeService.IsEmployeeValid(employee, createdById));
    }

    [Fact]
    public async Task IsEmployeeValid_ShouldReturnTrue_WhenEmployeeIsValid()
    {
        // Arrange
        var employee = new EmployeeDto
        {
            Role = (int)EmployeeRole.Employee,
            BirthDate = DateTime.UtcNow.AddYears(-20)
        };
        var createdById = Guid.NewGuid();
        var creator = new Employee("firstName", "lastName",  "email", "documentNumber", DateTime.UtcNow.AddYears(-20), new List<string>(), "password", EmployeeRole.Director, Guid.Empty);

        _employeeRepositoryMock.Setup(repo => repo.GetByIdAsync(createdById))
            .ReturnsAsync(creator);

        // Act
        var result = await _employeeService.IsEmployeeValid(employee, createdById);

        // Assert
        Assert.True(result);
    }
}