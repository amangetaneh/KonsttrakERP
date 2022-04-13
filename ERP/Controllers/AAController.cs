using ERP.Context;
using ERP.DTOs;
using ERP.Models;
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
        private readonly ISiteService _siteService;

        public AAController(DataContext context, IEquipmentAssetService equipmentAssetService, IItemSiteQtyService itemSiteQtyService, IReportService reportService)
        {
            this.context = context;
            _equipmentAssetService = equipmentAssetService;
            _itemSiteQtyService = itemSiteQtyService;
            _reportService = reportService;
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
        public async Task<ActionResult<List<Site>>> Get()
        {
            var issueItems = await context.IssueItems
                .Where(ii => ii.Issue.Status == ISSUESTATUS.HANDED)
                .Include(ii => ii.Issue)
                .Include(ii => ii.Item)
                .ThenInclude(i => i.Material)
                .ToListAsync();

            var grouped = issueItems.GroupBy(ii => ii.Issue.HandDate.Value.Date)
                .Select(s => new
                {
                    key = s.Key.ToShortDateString(),
                    all = s.GroupBy(ii => ii.ItemId)
                    .Select(q => new
                    {
                        ItemId = q.Key,
                        Qty = q.Sum(ii => ii.QtyApproved),
                        Cost = q.Sum(ii => ii.QtyApproved * ii.Cost),
                        CurrentCost = q.Sum(ii => ii.QtyApproved * ii.Item.Material.Cost)
                    })
                });

            var transferOutItems = await context.TransferItems
                .Where(ii => ii.Transfer.Status >= TRANSFERSTATUS.SENT && ii.Item.Type == ITEMTYPE.EQUIPMENT)
                .Include(ii => ii.Transfer)
                .Include(ii => ii.Item.Material)
                .Include(ii => ii.Item.Equipment.EquipmentModels)
                .ToListAsync();

            var groupedTransfer = transferOutItems.GroupBy(ii => ii.Transfer.SendDate.Value.Date)
                .Select(s => new
                {
                    key = s.Key.ToShortDateString(),
                    qty = s.Sum(ii => ii.QtyApproved),
                    cost = s.Sum(ii => ii.QtyApproved * ii.Cost),
                    currentValue = s.Sum(ii => ii.Item.Equipment == null ?
                        ii.QtyApproved * ii.Item.Material.Cost :
                        ii.QtyApproved * ii.Item.Equipment.EquipmentModels
                            .Where(em => em.EquipmentModelId == ii.EquipmentModelId).First().Cost)

                });

            HashSet<String> labels = new();

            foreach (var x in groupedTransfer)
                labels.Add(x.key);

            foreach (var x in grouped)
                labels.Add(x.key);

            labels = labels.OrderBy(x => DateTime.Parse(x)).ToHashSet();

            return Ok(new { labels, grouped, g = groupedTransfer.ToDictionary(g => g.key, g => g) });
        }
    }
}
