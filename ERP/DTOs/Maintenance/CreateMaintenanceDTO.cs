namespace ERP.DTOs
{
    public class CreateMaintenanceDTO
    {
                
        public int RequestedById { get; set; }

        public int StoreId { get; set; }

        public string Reason { get; set; }
   
        public int ItemId { get; set; }



    }
}
