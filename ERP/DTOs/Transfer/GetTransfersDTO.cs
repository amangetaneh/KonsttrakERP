namespace ERP.DTOs
{
    public class GetTransfersDTO
    {
        public int SiteId { get; set; } = -1;

        public int EmployeeId { get; set; } = -1;

        public string Type { get; set; } = "in";

        public int? SendSite;

        public int? ReceiveSite;
    
        public void SetSite()
        {
            if (Type == "in" && SiteId != -1)
                ReceiveSite = SiteId;
            else if (Type == "out" && SiteId != -1)
                SendSite = SiteId;
        }
    
    }


}
