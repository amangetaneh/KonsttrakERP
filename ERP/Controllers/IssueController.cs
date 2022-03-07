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

        public IssueController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Issue>> Get(int id)
        {
            var issue = context.Issues.Where(issue => issue.Id == id)
               .Include(issue => issue.IssueItems)
               .ThenInclude(issueItem => issueItem.Item)
               .ThenInclude(item => item.ConsumableItem)
               .Include(issue => issue.IssueItems)
               .ThenInclude(issueItem => issueItem.Item.NonConsumableItem)
               .FirstOrDefault();

            if (issue == null) return NotFound("Issue Not Found.");

            return Ok(issue);
        }

        [HttpGet]
        public async Task<ActionResult<List<Issue>>> Get()
        {

            var issues = await context.Issues.ToListAsync();

            return Ok(issues);
        }

        [HttpPost("issue")]
        public async Task<ActionResult<List<Issue>>> Post(CreateIssueDTO issueDTO)
        {
            Issue issue = new();
            issue.Date = issueDTO.Date;
            issue.RequestedById = issueDTO.RequestedById;

            ICollection<IssueItem> issueItems = new List<IssueItem>();

            foreach (var requestItem in issueDTO.IssueItems)
            {
                IssueItem issueItem = new();
                issueItem.ItemId = requestItem.ItemId;
                issueItem.QtyIssued = requestItem.QtyIssued;
                issueItem.Remark = requestItem.Remark;

                issueItems.Add(issueItem);
            }

            issue.IssueItems = issueItems;

            context.Issues.Add(issue);
            await context.SaveChangesAsync();

            return Ok(issue);
        }

        [HttpPost("approve")]
        public async Task<ActionResult<Issue>> Approve(int id, int approvedById)
        {
            var issue = await context.Issues.FindAsync(id);
            if (issue == null) return NotFound("Issue Not Found.");

            issue.ApprovedById = approvedById;
            issue.Status = 1;

            await context.SaveChangesAsync();

            return Ok(issue);
        }

        [HttpPost("hand")]
        public async Task<ActionResult<Transfer>> Hand(Issue request)
        {
            var issue = context.Issues
                .Where(i => i.Id == request.Id)
                .Include(i => i.IssueItems)
                .FirstOrDefault();
            if (issue == null) return NotFound("Issue Not Found.");

            issue.HandedById = request.HandedById;

            foreach (var requestItem in request.IssueItems)
            {
                var issueItem = issue.IssueItems.Where(iItem => iItem.ItemId == requestItem.ItemId).FirstOrDefault();
                if (issueItem == null) return NotFound($"Issued Item with Id {requestItem.ItemId} Not Found");

                issueItem.QtyHanded = requestItem.QtyHanded;
            }

            issue.Status = 2;

            await context.SaveChangesAsync();


            return Ok(issue);
        }

    }
}
