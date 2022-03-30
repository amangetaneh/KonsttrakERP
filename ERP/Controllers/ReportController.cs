using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly DataContext context;

        public ReportController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Site>>> Get()
        {
            var sites = await context.Sites.ToListAsync();

            return Ok(sites);
        }

        [HttpPost("transfer")]
        public async Task<ActionResult<List<Transfer>>> GetTransfers(TransferReportDTO transferDTO)
        {
            transferDTO.SetDates(); // parse request data to set what date is given
            transferDTO.SetEmployees();

            var transfers = await context.Transfers
                .Where(transfer =>
                //dates
                (transferDTO.RequestDateFrom == null || transfer.RequestDate >= transferDTO.RequestDateFrom) &&
                (transferDTO.RequestDateTo == null || transfer.RequestDate <= transferDTO.RequestDateTo) &&
                (transferDTO.ApproveDateFrom == null || transfer.ApproveDate >= transferDTO.ApproveDateFrom) &&
                (transferDTO.ApproveDateTo == null || transfer.ApproveDate <= transferDTO.ApproveDateTo) &&
                (transferDTO.SendDateFrom == null || transfer.SendDate >= transferDTO.SendDateFrom) &&
                (transferDTO.SendDateTo == null || transfer.SendDate <= transferDTO.SendDateTo) &&
                (transferDTO.ReceiveDateFrom == null || transfer.ReceiveDate >= transferDTO.ReceiveDateFrom) &&
                (transferDTO.ReceiveDateTo == null || transfer.ReceiveDate <= transferDTO.ReceiveDateTo) &&
                //sites
                (transferDTO.SendSiteId == -1 || transfer.SendSiteId == transferDTO.SendSiteId) &&
                (transferDTO.ReceiveSiteId == -1 || transfer.ReceiveSiteId == transferDTO.ReceiveSiteId) &&
                //status
                (transferDTO.Status == -1 || transfer.Status == transferDTO.Status) &&
                //single role one user
                (transferDTO.EmployeeRole == -1 || transferDTO.RequestedById == null || transfer.RequestedById == transferDTO.RequestedById) &&
                (transferDTO.EmployeeRole == -1 || transferDTO.ApprovedById == null || transfer.ApprovedById == transferDTO.ApprovedById) &&
                (transferDTO.EmployeeRole == -1 || transferDTO.SentById == null || transfer.SentById == transferDTO.SentById) &&
                (transferDTO.EmployeeRole == -1 || transferDTO.ReceivedById == null || transfer.ReceivedById == transferDTO.ReceivedById) &&
                //multiple role one user
                (transferDTO.EmployeeRole != -1 || transferDTO.EmployeeId == -1 || transfer.RequestedById == transferDTO.RequestedById || transfer.ApprovedById == transferDTO.ApprovedById || transfer.SentById == transferDTO.SentById || transfer.ReceivedById == transferDTO.ReceivedById) &&
                //item type
                (transferDTO.ItemType == -1 || transfer.TransferItems.Any(ti => ti.Item.Type == transferDTO.ItemType)) &&
                //equipment category
                (transferDTO.ItemType != ITEMTYPE.EQUIPMENT || transfer.TransferItems.Any(ti => ti.Item.Equipment.EquipmentCategoryId == transferDTO.EquipmentCategoryId))
                )
                .Include(transfer => transfer.SendSite)
                .Include(transfer => transfer.ReceiveSite)
                .Include(transfer => transfer.RequestedBy)
                .Include(transfer => transfer.SentBy)
                .Include(transfer => transfer.ReceivedBy)
                .Include(transfer => transfer.ApprovedBy)
                .ToListAsync();


            return Ok(transfers);
        }

    }
}
