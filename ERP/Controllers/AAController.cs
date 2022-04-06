using ERP.Context;
using ERP.Models;
using ERP.Services.AssetNumberServices;
using ERP.Services.SiteServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AAController : Controller
    {
        private readonly DataContext context;
        private readonly IAssetNumberService _assetNumberService;
        private readonly ISiteService _siteService;

        public AAController(DataContext context, IAssetNumberService assetNumberService)
        {
            this.context = context;
            _assetNumberService = assetNumberService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Site>>> Get()
        {
            return Ok(await _assetNumberService.GenerateAssetNumbers(4, 10));
        }
    }
}
