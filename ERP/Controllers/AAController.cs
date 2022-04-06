using ERP.Context;
using ERP.Models;
using ERP.Services.AssetNumberServices;
using ERP.Services.ItemSiteQtyServices;
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
        private readonly IItemSiteQtyService _itemSiteQtyService;
        private readonly ISiteService _siteService;

        public AAController(DataContext context, IAssetNumberService assetNumberService, IItemSiteQtyService itemSiteQtyService)
        {
            this.context = context;
            _assetNumberService = assetNumberService;
            _itemSiteQtyService = itemSiteQtyService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Site>>> Get()
        {
            return Ok(await _itemSiteQtyService.SubtractMaterial(1, 8, 1));
        }
    }
}
