using ERP.Context;
using ERP.DTOs.User;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly DataContext context;

        public EmployeeController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> Get()
        {
            var stores = await context.Employees.ToListAsync();

            return Ok(stores);
        }

        [HttpPost("changesite")]
        public async Task<ActionResult<Employee>> ChangeEmployeeSite(UpdateUserDTO userDTO)
        {
            var employee = context.Employees
                .Where(e => e.EmployeeId == userDTO.EmployeeId)
                .FirstOrDefault();

            if (employee == null) return NotFound("Employee Not Found.");

            employee.EmployeeSiteId = userDTO.SiteId;

            await context.SaveChangesAsync();

            return Ok(employee);
        }

        [HttpPost("changerole")]
        public async Task<ActionResult<Employee>> ChangeEmployeeRole(UpdateUserDTO userDTO)
        {
            var userAccount = context.UserAccounts
                .Where(e => e.EmployeeId == userDTO.EmployeeId)
                .FirstOrDefault();

            if (userAccount == null) return NotFound("Employee Not Found.");

            userAccount.UserRoleId = userDTO.RoleId;

            await context.SaveChangesAsync();

            return Ok(userAccount);
        }
    }
}
