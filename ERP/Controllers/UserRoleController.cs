using ERP.Context;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : Controller
    {
        private readonly DataContext context;

        public UserRoleController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserRole>>> Get()
        {
            var roles = await context.UserRoles.ToListAsync();
            
            return Ok(roles);
        }
    }
}
