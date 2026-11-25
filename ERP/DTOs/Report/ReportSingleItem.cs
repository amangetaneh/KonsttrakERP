namespace ERP.DTOs
{
    public class ReportSingleItem
    {
        public dynamic Key { get; set; } = null!;

        public dynamic Label { get; set; } = null!;

        public decimal? Qty { get; set; }

        public decimal? Cost { get; set; }

        public decimal? CurrentValue { get; set; }

    }
}
