using Employees.Domain.Dtos;
using Employees.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employees.WebAPI.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController(
    IAuthenticationService authenticationService,
    IEmployeeService employeeService)
    : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var token = await authenticationService.AuthenticateAsync(request.DocumentNumber, request.Password);
        if (token == "")
            return Unauthorized(new { message = "Invalid credentials" });

        var employee = await employeeService.GetByDocumentNumberAsync(request.DocumentNumber);
        
        return Ok(new { token, employee });
    }
}