using ERP.Context;

namespace ERP.DTOs.ReportServices
{
    public class ReportService : IReportService
    {
        private readonly DataContext _context;

        public ReportService(DataContext context)
        {
            _context = context;
        }

    }
}
