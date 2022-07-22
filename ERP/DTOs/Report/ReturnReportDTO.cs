namespace ERP.DTOs
{
    public class ReturnReportDTO
    {
        public int DateOf { get; set; } = -1;

        public string FromDate { get; set; } = "";

        public string ToDate { get; set; } = "";

        public int SiteId { get; set; } = -1;

        //public int DamageId { get; set; } = -1;

        public int EquipmentCategoryId { get; set; } = -1;

        public int ItemId { get; set; } = -1;

        public int EmployeeRole { get; set; } = -1;

        public int EmployeeId { get; set; } = -1;

        public string GroupBy { get; set; } = "";

        public DateTime? ReturnDateFrom;

        public DateTime? ReturnDateTo;

        public DateTime? BorrowDateFrom;

        public DateTime? BorrowDateTo;

        public int? ReturnedById;

        public int? BorrowedById;


        public void SetDates()
        {
            if (DateOf != -1 && FromDate != "")
            {
                DateTime fromDate = DateTime.Parse(FromDate);

                if (DateOf == RETURNREPORTTYPE.BORROW)
                    BorrowDateFrom = fromDate;
                else if (DateOf == RETURNREPORTTYPE.RETURN)
                    ReturnDateFrom = fromDate;
            }

            if (DateOf != -1 && ToDate != "")
            {
                DateTime toDate = DateTime.Parse(ToDate).AddDays(1);

                if (DateOf == RETURNREPORTTYPE.BORROW)
                    BorrowDateTo = toDate;
                else if (DateOf == RETURNREPORTTYPE.RETURN)
                    ReturnDateTo = toDate;
            }

        }

        public void SetEmployees()
        {
            if (EmployeeRole == -1 && EmployeeId != -1) // any role one employee
            {
                ReturnedById = EmployeeId;
                BorrowedById = EmployeeId;
                return;
            }

            if (EmployeeRole != -1 && EmployeeId != -1)
            {
                if (EmployeeRole == RETURNREPORTTYPE.BORROW)
                    BorrowedById = EmployeeId;
                else if (EmployeeRole == RETURNREPORTTYPE.RETURN)
                    ReturnedById = EmployeeId;

            }

        }
    }

}
