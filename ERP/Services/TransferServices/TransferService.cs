using ERP.Context;
using ERP.DTOs;
using ERP.Models;
using ERP.Services.NotificationServices;
using ERP.Services.User;
using Microsoft.EntityFrameworkCore;

namespace ERP.Services.TransferServices
{
    public class TransferService : ITransferService
    {
        private readonly DataContext _context;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;

        public TransferService(DataContext context, INotificationService notificationService, IUserService userService)
        {
            _context = context;
            _notificationService = notificationService;
            _userService = userService;
        }

        public async Task<Transfer> GetById(int id)
        {
            var transfer = await _context.Transfers.Where(transfer => transfer.TransferId == id)
                .Include(transfer => transfer.SendSite)
                .Include(transfer => transfer.ReceiveSite)
                .Include(transfer => transfer.RequestedBy)
                .Include(transfer => transfer.SentBy)
                .Include(transfer => transfer.ReceivedBy)
                .Include(transfer => transfer.ApprovedBy)
                .Include(transfer => transfer.TransferItems)
                .ThenInclude(transferItem => transferItem.Item)
                .ThenInclude(item => item.Material)
                .Include(transfer => transfer.TransferItems)
                .ThenInclude(transferItem => transferItem.Item.Equipment)
                .Include(transfer => transfer.TransferItems)
                .ThenInclude(transferItem => transferItem.TransferEquipmentAssets)
                .ThenInclude(transferEquipmentAsset => transferEquipmentAsset.EquipmentAsset)
                .FirstOrDefaultAsync();

            if (transfer == null) throw new KeyNotFoundException("Transfer Not Found.");

            return transfer;
        }

        public async Task<List<Transfer>> GetByCondition(GetTransfersDTO getTransfersDTO)
        {
            var transfers = await _context.Transfers
                .Include(transfer => transfer.SendSite)
                .Include(transfer => transfer.ReceiveSite)
                .ToListAsync();

            return transfers;
        }

        public async Task<Transfer> RequestEquipment (CreateEquipmentTransferDTO transferDTO)
        {
            Transfer transfer = new();
            transfer.SendSiteId = transferDTO.SendSiteId;
            transfer.ReceiveSiteId = transferDTO.ReceiveSiteId;
            transfer.RequestedById = _userService.Employee.EmployeeId;

            ICollection<TransferItem> transferItems = new List<TransferItem>();

            foreach (var requestItem in transferDTO.TransferItems)
            {
                TransferItem transferItem = new();
                transferItem.ItemId = requestItem.ItemId;
                transferItem.QtyRequested = requestItem.QtyRequested;
                transferItem.RequestRemark = requestItem.RequestRemark;

                var itemTemp = await _context.Items.Where(item => item.ItemId == requestItem.ItemId)
                    .Include(i => i.Equipment)
                    .Include(i => i.Material)
                    .FirstOrDefaultAsync();

                if (itemTemp == null) throw new KeyNotFoundException($"Transfer Item with Id {requestItem.ItemId} Not Found");

                if (itemTemp.Type != ITEMTYPE.EQUIPMENT) throw new InvalidOperationException($"Transfer Item with Id {requestItem.ItemId} Is Not Type of Equipment");

                transferItem.EquipmentModelId = requestItem.EquipmentModelId;

                var transferEquipmentModel = await _context.EquipmentModels
                    .Where(equipModel => equipModel.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefaultAsync();

                if (transferEquipmentModel == null) throw new KeyNotFoundException($"Transfer Item Model with Id {requestItem.EquipmentModelId} Not Found");

                transferItem.Cost = transferEquipmentModel.Cost;

                transferItems.Add(transferItem);
            }

            transfer.TransferItems = transferItems;

            _context.Transfers.Add(transfer);
            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.TRANSFER,
                status: transfer.Status,
                actionId: transfer.TransferId,
                siteId: transfer.SendSiteId,
                employeeId: transfer.RequestedById);

            return transfer;
        }

        public async Task<Transfer> RequestMaterial (CreateMaterialTransferDTO transferDTO)
        {
            Transfer transfer = new();
            transfer.SendSiteId = transferDTO.SendSiteId;
            transfer.ReceiveSiteId = transferDTO.ReceiveSiteId;
            transfer.RequestedById = _userService.Employee.EmployeeId;

            ICollection<TransferItem> transferItems = new List<TransferItem>();

            foreach (var requestItem in transferDTO.TransferItems)
            {
                TransferItem transferItem = new();
                transferItem.ItemId = requestItem.ItemId;
                transferItem.QtyRequested = requestItem.QtyRequested;
                transferItem.RequestRemark = requestItem.RequestRemark;

                var itemTemp = await _context.Items.Where(item => item.ItemId == requestItem.ItemId).
                    Include(i => i.Equipment).
                    Include(i => i.Material).
                    FirstOrDefaultAsync();

                if (itemTemp == null) throw new KeyNotFoundException($"Transfer Item with Id {requestItem.ItemId} Not Found");

                if (itemTemp.Type != ITEMTYPE.MATERIAL) throw new InvalidOperationException($"Transfer Item with Id {requestItem.ItemId} Is Not Type of Material");

                transferItem.Cost = itemTemp.Material.Cost;

                transferItems.Add(transferItem);
            }

            transfer.TransferItems = transferItems;

            _context.Transfers.Add(transfer);
            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.TRANSFER,
                 status: transfer.Status,
                 actionId: transfer.TransferId,
                 siteId: transfer.SendSiteId,
                 employeeId: transfer.RequestedById);

            return transfer;
        }

        public async Task<Transfer> ApproveTransfer(ApproveTransferDTO approveDTO)
        {
            var transfer = await _context.Transfers
               .Where(t => t.TransferId == approveDTO.TransferId)
               .Include(t => t.TransferItems)
               .ThenInclude(ti => ti.Item)
               .FirstOrDefaultAsync();
            if (transfer == null) throw new KeyNotFoundException("Transfer Not Found.");

            transfer.ApproveDate = DateTime.Now;
            transfer.ApprovedById = _userService.Employee.EmployeeId;

            foreach (var requestItem in approveDTO.TransferItems)
            {
                var transferItem = transfer.TransferItems
                    .Where(ti => ti.ItemId == requestItem.ItemId && ti.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefault();

                if (transferItem == null) throw new KeyNotFoundException($"Transfer Item with Id {requestItem.ItemId} Not Found");

                transferItem.QtyApproved = requestItem.QtyApproved;
                transferItem.ApproveRemark = requestItem.ApproveRemark;
            }

            transfer.Status = TRANSFERSTATUS.APPROVED;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.TRANSFER,
                 status: transfer.Status,
                 actionId: transfer.TransferId,
                 siteId: transfer.SendSiteId,
                 employeeId: transfer.RequestedById);

            return transfer;
        }

        public async Task<Transfer> DeclineTransfer(DeclineTransferDTO declineDTO)
        {
            var transfer = await _context.Transfers
               .Where(t => t.TransferId == declineDTO.TransferId)
               .Include(t => t.TransferItems)
               .FirstOrDefaultAsync();
            if (transfer == null) throw new KeyNotFoundException("Transfer Not Found.");

            transfer.ApproveDate = DateTime.Now;
            transfer.ApprovedById = _userService.Employee.EmployeeId;

            foreach (var requestItem in declineDTO.TransferItems)
            {
                var transferItem = transfer.TransferItems
                    .Where(ti => ti.ItemId == requestItem.ItemId && ti.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefault();
                if (transferItem == null) throw new KeyNotFoundException($"Transfer Item with Id {requestItem.ItemId} Not Found");

                transferItem.ApproveRemark = requestItem.ApproveRemark;
            }

            transfer.Status = TRANSFERSTATUS.DECLINED;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.TRANSFER,
                status: transfer.Status,
                actionId: transfer.TransferId,
                siteId: transfer.SendSiteId,
                employeeId: transfer.RequestedById);

            return transfer;
        }

        public async Task<Transfer> SendTransfer(SendTransferDTO sendDTO)
        {

            var transfer = _context.Transfers
               .Where(t => t.TransferId == sendDTO.TransferId)
               .Include(t => t.TransferItems)
               .ThenInclude(ti => ti.Item)
               .FirstOrDefault();
            if (transfer == null) throw new KeyNotFoundException("Transfer Not Found.");

            transfer.SendDate = DateTime.Now;
            transfer.SentById = _userService.Employee.EmployeeId;

            foreach (var requestItem in sendDTO.TransferItems)
            {
                var transferItem = transfer.TransferItems
                    .Where(ti => ti.ItemId == requestItem.ItemId && ti.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefault();
                if (transferItem == null)
                    throw new KeyNotFoundException($"Transfer Item with Item Id {requestItem.ItemId} and Model Id {requestItem.EquipmentModelId} Not Found");

                transferItem.SendRemark = requestItem.SendRemark;

                if (transferItem.Item.Type == ITEMTYPE.EQUIPMENT && requestItem.EquipmentAssetIds != null)
                {
                    transferItem.TransferEquipmentAssets = new List<TransferItemEquipmentAsset>();

                    foreach (var requestAssetId in requestItem.EquipmentAssetIds)
                    {
                        TransferItemEquipmentAsset equipmentAsset = new();
                        equipmentAsset.EquipmentAssetId = requestAssetId;

                        transferItem.TransferEquipmentAssets.Add(equipmentAsset);
                    }
                }
            }

            transfer.Status = TRANSFERSTATUS.SENT;

            await _context.SaveChangesAsync();

            await _notificationService.Add(type: NOTIFICATIONTYPE.TRANSFER,
                status: transfer.Status,
                actionId: transfer.TransferId,
                siteId: transfer.SendSiteId,
                employeeId: transfer.RequestedById);

            return transfer;
        }

        public async Task<Transfer> ReceiveTransfer(ReceiveTransferDTO receiveDTO)
        {
            var transfer = _context.Transfers
               .Where(t => t.TransferId == receiveDTO.TransferId)
               .Include(t => t.TransferItems)
               .FirstOrDefault();
            if (transfer == null) throw new KeyNotFoundException("Transfer Not Found.");

            transfer.ReceiveDate = DateTime.Now;
            transfer.ReceivedById = _userService.Employee.EmployeeId;
            transfer.DeliveredById = receiveDTO.DeliveredById;
            transfer.VehiclePlateNo = receiveDTO.VehiclePlateNo;


            foreach (var requestItem in receiveDTO.TransferItems)
            {
                var transferItem = transfer.TransferItems
                    .Where(ti => ti.ItemId == requestItem.ItemId && ti.EquipmentModelId == requestItem.EquipmentModelId)
                    .FirstOrDefault();
                if (transferItem == null) throw new KeyNotFoundException($"Transfer Item with Id {requestItem.ItemId} and Model Id {requestItem.EquipmentModelId} Not Found");

                transferItem.ReceiveRemark = requestItem.ReceiveRemark;
            }

            transfer.Status = TRANSFERSTATUS.RECEIVED;

            await _context.SaveChangesAsync();

            return transfer;
        }
    }
}
