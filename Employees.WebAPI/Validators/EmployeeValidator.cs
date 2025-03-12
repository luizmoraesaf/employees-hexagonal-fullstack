using Employees.Domain.Dtos;
using FluentValidation;

namespace Employees.WebAPI.Validators;

public class EmployeeValidator : AbstractValidator<EmployeeDto>
{
    public EmployeeValidator()
    {
        RuleFor(employee => employee.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must be less than 50 characters.");

        RuleFor(employee => employee.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must be less than 50 characters.");

        RuleFor(employee => employee.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(employee => employee.DocumentNumber)
            .NotEmpty().WithMessage("Document number is required.")
            .MaximumLength(20).WithMessage("Document number must be less than 20 characters.");

        RuleFor(employee => employee.BirthDate)
            .NotEmpty().WithMessage("Birth date is required.")
            .LessThan(DateTime.Now).WithMessage("Birth date must be in the past.");

        RuleFor(employee => employee.Role)
            .InclusiveBetween(1, 3).WithMessage("Role must be a valid role between 1 and 3.");

        RuleFor(employee => employee.Phones)
            .NotEmpty().WithMessage("At least one phone number is required.");

        RuleFor(employee => employee.ManagerId)
            .Must(managerId => string.IsNullOrEmpty(managerId) || Guid.TryParse(managerId, out _))
            .WithMessage("Invalid manager ID format.");
        
        RuleFor(employee => employee.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
    }
}