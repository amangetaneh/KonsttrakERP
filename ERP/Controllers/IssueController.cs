using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class IssueController : Controller
    {
        private readonly DataContext context;
        private readonly NotificationManager notificationManager;

        public IssueController(DataContext context, NotificationManager notificationManager)
        {
            this.context = context;
            this.notificationManager = notificationManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Issue>> Get(int id)
        {
            var issue = context.Issues.Where(issue => issue.IssueId == id)
                .Include(issue => issue.Site)
                .Include(issue => issue.RequestedBy)
                .Include(issue => issue.ApprovedBy)
                .Include(issue => issue.HandedBy)
               .Include(issue => issue.IssueItems)
               .ThenInclude(issueItem => issueItem.Item)
               .ThenInclude(item => item.Material)
               .Include(issue => issue.IssueItems)
               .ThenInclude(issueItem => issueItem.Item.Equipment)
               .FirstOrDefault();

            if (issue == null) return NotFound("Issue Not Found.");

            return Ok(issue);
        }

        [HttpGet]
        public async Task<ActionResult<List<Issue>>> Get()
        {

            var issues = await context.Issues
                .Include(issue => issue.Site)
                .ToListAsync();

            return Ok(issues);
        }

        [HttpPost("request")]
        public async Task<ActionResult<List<Issue>>> Post(CreateIssueDTO issueDTO)
        {
            Issue issue = new();
            issue.RequestedById = issueDTO.RequestedById;
            issue.SiteId = issueDTO.SiteId;

            ICollection<IssueItem> issueItems = new List<IssueItem>();

            foreach (var requestItem in issueDTO.IssueItems)
            {
                IssueItem issueItem = new();
                issueItem.ItemId = requestItem.ItemId;
                issueItem.QtyRequested = requestItem.QtyRequested;
                issueItem.RequestRemark = requestItem.RequestRemark;

                var itemTemp = context.Items.Where(item => item.ItemId == requestItem.ItemId).
                   Include(i => i.Equipment).
                   Include(i => i.Material).
                   FirstOrDefault();

                if (itemTemp == null) return NotFound($"Issue Item with Id {requestItem.ItemId} Not Found");

                if (itemTemp.Type == ITEMTYPE.MATERIAL)
                    issueItem.Cost = itemTemp.Material.Cost;
                // else

                /////////////////////////////////////



                issueItems.Add(issueItem);
            }

            issue.IssueItems = issueItems;

            context.Issues.Add(issue);
            await context.SaveChangesAsync();

            await notificationManager.Add(type: NOTIFICATIONTYPE.ISSUE,
                status: issue.Status,
                actionId: issue.IssueId,
                siteId: issue.SiteId,
                employeeId: issue.RequestedById);

            return Ok(issue.IssueId);
        }

        [HttpPost("approve")]
        public async Task<ActionResult<Issue>> Approve(ApproveIssueDTO approveDTO)
        {
            var issue = await context.Issues
                .Where(i => i.IssueId == approveDTO.IssueId)
                .Include(i => i.IssueItems)
                .FirstOrDefaultAsync();
            if (issue == null) return NotFound("Issue Not Found.");

            issue.ApproveDate = DateTime.Now;
            issue.ApprovedById = approveDTO.ApprovedById;

            foreach (var requestItem in approveDTO.IssueItems)
            {
                var isssueItem = issue.IssueItems
                    .Where(tItem => tItem.ItemId == requestItem.ItemId)
                    .FirstOrDefault();

                if (isssueItem == null) return NotFound($"Issue Item with Id {requestItem.ItemId} Not Found");

                isssueItem.QtyApproved = requestItem.QtyApproved;
                isssueItem.ApproveRemark = requestItem.ApproveRemark;
            }

            issue.Status = ISSUESTATUS.APPROVED;

            await context.SaveChangesAsync();

            await notificationManager.Add(type: NOTIFICATIONTYPE.ISSUE,
                status: issue.Status,
                actionId: issue.IssueId,
                siteId: issue.SiteId,
                employeeId: issue.RequestedById);

            return Ok(issue);
        }

        [HttpPost("decline")]
        public async Task<ActionResult<Issue>> Decline(DeclineIssueDTO declineDTO)
        {
            var issue = await context.Issues
               .Where(i => i.IssueId == declineDTO.IssueId)
               .Include(i => i.IssueItems)
               .FirstOrDefaultAsync();
            if (issue == null) return NotFound("Issue Not Found.");

            issue.ApproveDate = DateTime.Now;
            issue.ApprovedById = declineDTO.ApprovedById;

            foreach (var requestItem in declineDTO.IssueItems)
            {
                var issueItem = issue.IssueItems.Where(tItem => tItem.ItemId == requestItem.ItemId).FirstOrDefault();
                if (issueItem == null) return NotFound($"Issue Item with Id {requestItem.ItemId} Not Found");

                issueItem.ApproveRemark = requestItem.ApproveRemark;
            }

            issue.Status = ISSUESTATUS.DECLINED;

            await context.SaveChangesAsync();

            await notificationManager.Add(type: NOTIFICATIONTYPE.ISSUE,
                status: issue.Status,
                actionId: issue.IssueId,
                siteId: issue.SiteId,
                employeeId: issue.RequestedById);

            return Ok(issue);
        }

        [HttpPost("hand")]
        public async Task<ActionResult<Issue>> Hand(HandIssueDTO handDTO)
        {
            var issue = context.Issues
                .Where(i => i.IssueId == handDTO.IssueId)
                .Include(i => i.IssueItems)
                .FirstOrDefault();
            if (issue == null) return NotFound("Issue Not Found.");

            issue.HandDate = DateTime.Now;
            issue.HandedById = handDTO.HandedById;

            foreach (var requestItem in handDTO.IssueItems)
            {
                var issueItem = issue.IssueItems.Where(tItem => tItem.ItemId == requestItem.ItemId).FirstOrDefault();
                if (issueItem == null) return NotFound($"Issue Item with Id {requestItem.ItemId} Not Found");

                issueItem.HandRemark = requestItem.HandRemark;
            }

            issue.Status = ISSUESTATUS.HANDED;

            await context.SaveChangesAsync();

            await notificationManager.Add(type: NOTIFICATIONTYPE.ISSUE,
                status: issue.Status,
                actionId: issue.IssueId,
                siteId: issue.SiteId,
                employeeId: issue.RequestedById);

            return Ok(issue);
        }

    }
}
