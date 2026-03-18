using Maintenance.WebAPI.Models;

namespace Maintenance.WebAPI.Services;

public class JSFakeRepairHistoryService : IJSRepairHistoryService
{
    // In-memory storage (state)
    private static readonly List<JSRepairHistoryDto> _repairs = new();
    private static int _nextId = 1;

    public JSFakeRepairHistoryService()
    {
        // Seed sample data once (only first time)
        if (_repairs.Count == 0)
        {
            _repairs.Add(new JSRepairHistoryDto
            {
                Id = _nextId++,
                VehicleId = 1,
                RepairDate = DateTime.Now.AddDays(-10),
                Description = "Oil change",
                Cost = 89.99m,
                PerformedBy = "Quick Lube"
            });

            _repairs.Add(new JSRepairHistoryDto
            {
                Id = _nextId++,
                VehicleId = 1,
                RepairDate = DateTime.Now.AddDays(-40),
                Description = "Brake pad replacement",
                Cost = 350.00m,
                PerformedBy = "Auto Repair Pro"
            });
        }
    }

    public List<JSRepairHistoryDto> GetByVehicleId(int vehicleId)
    {
        return _repairs.Where(r => r.VehicleId == vehicleId).ToList();
    }

    public JSRepairHistoryDto AddRepair(JSRepairHistoryDto repair)
    {
        // Assign new Id
        repair.Id = _nextId++;

        // If repair date not provided, set now
        if (repair.RepairDate == default)
            repair.RepairDate = DateTime.Now;

        _repairs.Add(repair);
        return repair;
    }
}
