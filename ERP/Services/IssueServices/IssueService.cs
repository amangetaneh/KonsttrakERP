using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using ERP.Services.NotificationServices;
using ERP.Services.User;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.IssueServices
{
    public class IssueService: IIssueService
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public IssueService(DataContext context, IUserService userService, INotificationService notificationService)
        {
            _context = context;
            _userService = userService;
            _notificationService = notificationService;
        }

        public async Task<Issue> GetById(int id)
        {
            var issue = await _context.Issues.Where(issue => issue.IssueId == id)
                .Include(issue => issue.Site)
                .Include(issue => issue.RequestedBy)
                .Include(issue => issue.ApprovedBy)
                .Include(issue => issue.HandedBy)
               .Include(issue => issue.IssueItems)
               .ThenInclude(issueItem => issueItem.Item)
               .ThenInclude(item => item.Material)
               .Include(issue => issue.IssueItems)
               .ThenInclude(issueItem => issueItem.Item.Equipment)
               .FirstOrDefaultAsync();

            if (issue == null) throw new KeyNotFoundException("Issue Not Found.");

            return issue;
        }
        private void checkEmployeeSiteIsAvailable()
        {
            if (_userService.Employee.EmployeeSite == null)
                throw new InvalidOperationException("Borrowing Employee Does Not Have A Site");
        }

        public async Task<Issue> RequestIssue(CreateIssueDTO issueDTO)
        {
            checkEmployeeSiteIsAvailable();

            Issue issue = new();
            issue.RequestedById = _userService.Employee.EmployeeId;
            issue.SiteId = (int)_userService.Employee.EmployeeSiteId;

            ICollection<IssueItem> issueItems = new List<IssueItem>();

            foreach (var requestItem in issueDTO.IssueItems)
            {
                IssueItem issueItem = new();
                issueItem.ItemId = requestItem.ItemId;
                issueItem.QtyRequested = requestItem.QtyRequested;
                issueItem.RequestRemark = requestItem.RequestRemark;

                var itemTemp = _context.Items.Where(item => item.ItemId == requestItem.ItemId).
                   Include(i => i.Equipment).
                   Include(i => i.Material).
                   FirstOrDefault();

                if (itemTemp == null) throw new KeyNotFoundException($"Issue Item with Id {requestItem.ItemId} Not Found");

                if (itemTemp.Type != ITEMTYPE.MATERIAL) throw new InvalidOperationException($"Borrow Item with Id {requestItem.ItemId} Is Not Type of Material");

                issueItem.Cost = itemTemp.Material.Cost;

                issueItems.Add(issueItem);
            }

            issue.IssueItems = issueItems;

            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.ISSUE,
                status: issue.Status,
                actionId: issue.IssueId,
                siteId: issue.SiteId,
                employeeId: issue.RequestedById);

            return issue;
        }


        public async Task<Issue> ApproveIssue(ApproveIssueDTO approveDTO)
        {
            var issue = await _context.Issues
                .Where(i => i.IssueId == approveDTO.IssueId)
                .Include(i => i.IssueItems)
                .FirstOrDefaultAsync();
            if (issue == null) throw new KeyNotFoundException("Issue Not Found.");

            issue.ApproveDate = DateTime.Now;
            issue.ApprovedById = _userService.Employee.EmployeeId;

            foreach (var requestItem in approveDTO.IssueItems)
            {
                var isssueItem = issue.IssueItems
                    .Where(tItem => tItem.ItemId == requestItem.ItemId)
                    .FirstOrDefault();

                if (isssueItem == null) throw new KeyNotFoundException($"Issue Item with Id {requestItem.ItemId} Not Found");

                isssueItem.QtyApproved = requestItem.QtyApproved;
                isssueItem.ApproveRemark = requestItem.ApproveRemark;
            }

            issue.Status = ISSUESTATUS.APPROVED;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.ISSUE,
                status: issue.Status,
                actionId: issue.IssueId,
                siteId: issue.SiteId,
                employeeId: issue.RequestedById);

            return issue;
        }

        public async Task<Issue> DeclineIssue(DeclineIssueDTO declineDTO)
        {
            var issue = await _context.Issues
               .Where(i => i.IssueId == declineDTO.IssueId)
               .Include(i => i.IssueItems)
               .FirstOrDefaultAsync();
            if (issue == null) throw new KeyNotFoundException("Issue Not Found.");

            issue.ApproveDate = DateTime.Now;
            issue.ApprovedById = _userService.Employee.EmployeeId;

            foreach (var requestItem in declineDTO.IssueItems)
            {
                var issueItem = issue.IssueItems.Where(tItem => tItem.ItemId == requestItem.ItemId).FirstOrDefault();
                if (issueItem == null) throw new KeyNotFoundException($"Issue Item with Id {requestItem.ItemId} Not Found");

                issueItem.ApproveRemark = requestItem.ApproveRemark;
            }

            issue.Status = ISSUESTATUS.DECLINED;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.ISSUE,
                status: issue.Status,
                actionId: issue.IssueId,
                siteId: issue.SiteId,
                employeeId: issue.RequestedById);

            return issue;

        }

        public async Task<Issue> HandIssue(HandIssueDTO handDTO)
        {
            var issue = _context.Issues
                .Where(i => i.IssueId == handDTO.IssueId)
                .Include(i => i.IssueItems)
                .FirstOrDefault();
            if (issue == null) throw new KeyNotFoundException("Issue Not Found.");

            issue.HandDate = DateTime.Now;
            issue.HandedById = _userService.Employee.EmployeeId;

            foreach (var requestItem in handDTO.IssueItems)
            {
                var issueItem = issue.IssueItems.Where(tItem => tItem.ItemId == requestItem.ItemId).FirstOrDefault();
                if (issueItem == null) throw new KeyNotFoundException($"Issue Item with Id {requestItem.ItemId} Not Found");

                issueItem.HandRemark = requestItem.HandRemark;
            }

            issue.Status = ISSUESTATUS.HANDED;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.ISSUE,
                status: issue.Status,
                actionId: issue.IssueId,
                siteId: issue.SiteId,
                employeeId: issue.RequestedById);

            return issue;
        }
    }
}
