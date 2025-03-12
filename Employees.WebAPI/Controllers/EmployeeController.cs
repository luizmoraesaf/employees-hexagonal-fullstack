using Employees.Domain.Dtos;
using Employees.Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employees.WebAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger, IValidator<EmployeeDto> validator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EmployeeDto employee)
    {
        var validationResult = await validator.ValidateAsync(employee);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        try
        {
            var userId = User.Claims.First().Value;
            var guidEmployee = await employeeService.CreateAsync(employee, Guid.Parse(userId));
            return CreatedAtAction(nameof(GetById), new { id = guidEmployee }, employee);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating employee");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var employee = await employeeService.GetByIdAsync(id);
            if (employee == null)
                return NotFound();

            return Ok(employee);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving employee");
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var employee = await employeeService.GetByIdAsync(Guid.Parse(id));
            if (employee == null)
                return NotFound();

            await employeeService.DeleteAsync(Guid.Parse(id));
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting employee");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] EmployeeDto employee, [FromRoute] string id)
    {
        var validationResult = await validator.ValidateAsync(employee);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        try
        {
            var userId = User.Claims.First().Value;
            employee.Id = id;
            await employeeService.UpdateAsync(employee, Guid.Parse(userId));
            return NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating employee");
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var employees = await employeeService.GetAllAsync();
            return Ok(employees);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving employees");
            return StatusCode(500, ex.Message);
        }
    }
}