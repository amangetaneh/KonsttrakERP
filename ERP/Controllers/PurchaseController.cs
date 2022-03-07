using ERP.Context;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class PurchaseController : Controller
    {
        private readonly DataContext context;

        public PurchaseController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Purchase>>> Get()
        {

            var purchases = await context.Purchases.Include(t => t.PurchaseItems).ToListAsync();

            return Ok(purchases);
        }

        [HttpPost]
        public async Task<ActionResult<List<Purchase>>> Post(Purchase singlePurchase)
        {

            context.Purchases.Add(singlePurchase);
            await context.SaveChangesAsync();

            return Ok(context.Purchases.FindAsync(singlePurchase.Id));
        }

    }
}
