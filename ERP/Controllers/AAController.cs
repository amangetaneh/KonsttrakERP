using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using ERP.Services.AssetNumberServices;
using ERP.Services.EquipmentAssetServices;
using ERP.Services.ItemSiteQtyServices;
using ERP.Services.ReportServices;
using ERP.Services.SiteServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AAController : Controller
    {
        private readonly DataContext context;
        private readonly IEquipmentAssetService _equipmentAssetService;
        private readonly IItemSiteQtyService _itemSiteQtyService;
        private readonly IReportService _reportService;
        private readonly IAssetNumberService _assetNumberService;
        private readonly ISiteService _siteService;

        public AAController(DataContext context, IEquipmentAssetService equipmentAssetService, IItemSiteQtyService itemSiteQtyService, IReportService reportService, IAssetNumberService assetNumberService)
        {
            this.context = context;
            _equipmentAssetService = equipmentAssetService;
            _itemSiteQtyService = itemSiteQtyService;
            _reportService = reportService;
            _assetNumberService = assetNumberService;
        }

        [HttpGet("a")]
        public async Task<ActionResult> GetA()
        {

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Post(GeneralReportDTO reportDTO)
        {           
            
            return Ok(await _reportService.GetGeneralReport(reportDTO));
        }

        [HttpGet]
        public async Task<ActionResult<List<Site>>> Get(int itemId, int count)
        {

            return Ok(await _assetNumberService.GenerateAssetNumbers(itemId, count));
        }
    }
}
