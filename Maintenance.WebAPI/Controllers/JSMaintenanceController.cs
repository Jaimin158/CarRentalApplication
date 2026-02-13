using Maintenance.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Maintenance.WebAPI.Controllers;

[ApiController]
[Route("api/maintenance")]
public class JSMaintenanceController : ControllerBase
{
    private readonly IJSRepairHistoryService _service;

    public JSMaintenanceController(IJSRepairHistoryService service)
    {
        _service = service;
    }

    [HttpGet("vehicles/{vehicleId}/repairs")]
    public IActionResult GetRepairHistory(int vehicleId)
    {
        var history = _service.GetByVehicleId(vehicleId);
        return Ok(history);
    }
}
