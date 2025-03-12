using Employees.Domain.Entities;
using Employees.Domain.Enums;
using Employees.Infrastructure.Data;
using Employees.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace Employees.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.Migrate(); // Aplica migrations pendentes

        if (!context.Employees.Any()) // Verifica se o banco está vazio
        {
            var passwordHasher = new BcryptPasswordHasher(); // Usa a classe de hashing

            var director = context.Employees.Add(new("Admin", "User", "admin@example.com", "12345678901",
                new DateTime(1990, 5, 10), ["999999998"], passwordHasher.HashPassword("Admin@123"), EmployeeRole.Director,
                Guid.Empty));

            var manager = context.Employees.Add(new("Manager", "User", "manager@example.com", "10987654321",
                new DateTime(1990, 5, 10), ["999999999"], passwordHasher.HashPassword("Manager@123"), EmployeeRole.Leader,
                director.Entity.Id));

            context.Employees.Add(new("Employee", "User", "employee@example.com", "12345678910",
                new DateTime(1990, 5, 10), ["999999991"], passwordHasher.HashPassword("Employee@123"), EmployeeRole.Employee, manager.Entity.Id));

            context.SaveChanges();
        }
    }
}