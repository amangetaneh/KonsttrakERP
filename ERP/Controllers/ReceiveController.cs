using ERP.Context;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class ReceiveController : Controller
    {
        private readonly DataContext context;

        public ReceiveController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Receive>>> Get()
        {

            var receives = await context.Receives.Include(t => t.ReceiveItems).ToListAsync();

            return Ok(receives);
        }

        [HttpPost]
        public async Task<ActionResult<List<Receive>>> Post(Receive singleReceive)
        {

            context.Receives.Add(singleReceive);
            await context.SaveChangesAsync();

            return Ok(context.Receives.FindAsync(singleReceive.Id));
        }

    }
}
