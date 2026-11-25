using System.Globalization;

namespace ERP.Services.ReportServices
{
    internal static class ReportDates
    {
        // Existing report keys use MinValue for a missing date. Label that group explicitly.
        public static string Day(DateTime date) => date == DateTime.MinValue ? "N/A" : date.ToShortDateString();
        public static string Week(int week, int year) => year <= 1 ? "N/A" : $"Week {week}, {year}";
        public static string Month(DateTime date) => date == DateTime.MinValue ? "N/A" : date.ToString("MMMM, yyyy", CultureInfo.CurrentCulture);
        public static string Year(int year) => year <= 1 ? "N/A" : year.ToString(CultureInfo.CurrentCulture);
    }
}
