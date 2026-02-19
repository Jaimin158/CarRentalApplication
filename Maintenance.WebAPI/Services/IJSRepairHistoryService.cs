using Maintenance.WebAPI.Models;

namespace Maintenance.WebAPI.Services;

public interface IJSRepairHistoryService
{
    List<JSRepairHistoryDto> GetByVehicleId(int vehicleId);
    JSRepairHistoryDto AddRepair(JSRepairHistoryDto repair);
}
