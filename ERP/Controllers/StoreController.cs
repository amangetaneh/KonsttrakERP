using ERP.Context;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteController : Controller
    {
        private readonly DataContext context;

        public SiteController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Site>>> Get()
        {
            var sites = await context.Sites.ToListAsync();

            return Ok(sites);
        }
    }
}
