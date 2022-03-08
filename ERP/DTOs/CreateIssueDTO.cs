namespace ERP.DTOs
{
    public class CreateIssueDTO
    {
                
        public int RequestedById { get; set; }

        public ICollection<CreateIssueItem> IssueItems { get; set; }
    }

    public class CreateIssueItem
    {
        public int ItemId { get; set; }

        public int QtyIssued { get; set; }

        public string Remark { get; set; } = string.Empty;


    }
}
