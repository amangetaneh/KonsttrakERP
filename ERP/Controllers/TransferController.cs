using ERP.Context;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class TransferController : Controller
    {
        private readonly DataContext context;

        public TransferController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Transfer>>> Get()
        {

            var transfers = await context.Transfers.Include(t => t.TransferItems).ToListAsync();

            return Ok(transfers);
        }

        [HttpPost]
        public async Task<ActionResult<List<Transfer>>> Post(Transfer transfer)
        {
            
            context.Transfers.Add(transfer);
            await context.SaveChangesAsync();

            return Ok(context.Transfers.FindAsync(transfer.Id));
        }

    }
}
