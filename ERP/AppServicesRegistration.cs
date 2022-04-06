
using ERP.DTOs.ReportServices;
using ERP.Services.AssetNumberServices;
using ERP.Services.BorrowServices;
using ERP.Services.EquipmentCategoryServices;
using ERP.Services.IssueServices;
using ERP.Services.ItemServices;
using ERP.Services.MaintenanceServices;
using ERP.Services.NotificationServices;
using ERP.Services.ReturnServices;
using ERP.Services.SiteServices;
using ERP.Services.TransferServices;

namespace ERP
{
    public static class AppServiceRegistration
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ITransferService, TransferService>();
            services.AddScoped<IBorrowService, BorrowService>();
            services.AddScoped<IIssueService, IssueService>();
            services.AddScoped<IMaintenanceService, MaintenanceService>();
            services.AddScoped<IReturnService, ReturnService>();
            services.AddScoped<ISiteService, SiteService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IEquipmentCategoryService, EquipmentCategoryService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IAssetNumberService, AssetNumberService>();

        }
    }
}