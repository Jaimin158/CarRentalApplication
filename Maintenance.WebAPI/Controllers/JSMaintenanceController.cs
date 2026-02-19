using Maintenance.WebAPI.Models;
using Maintenance.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Maintenance.WebAPI.Controllers;

[ApiController]
[Route("api/maintenance")]
public class JSMaintenanceController : ControllerBase
{
    private readonly IJSRepairHistoryService _service;
    private readonly Dictionary<string, int> _usageCounts;

    public JSMaintenanceController(IJSRepairHistoryService service, Dictionary<string, int> usageCounts)
    {
        _service = service;
        _usageCounts = usageCounts;
    }

    [HttpGet("crash")]
    public IActionResult Crash()
    {
        int x = 0;
        int y = 5 / x;
        return Ok();
    }


    [HttpGet("vehicles/{vehicleId}/repairs")]
    public IActionResult GetRepairHistory(int vehicleId)
    {
        var history = _service.GetByVehicleId(vehicleId);
        return Ok(history);
    }

    [HttpPost("repairs")]
    public IActionResult AddRepair([FromBody] JSRepairHistoryDto repair)
    {
        if (repair.VehicleId <= 0)
        {
            return BadRequest(new
            {
                error = "InvalidParameter",
                message = "VehicleId must be greater than zero."
            });
        }

        if (string.IsNullOrWhiteSpace(repair.Description))
        {
            return BadRequest(new
            {
                error = "InvalidParameter",
                message = "Description must not be empty."
            });
        }

        if (repair.Cost < 0)
        {
            return BadRequest(new
            {
                error = "InvalidParameter",
                message = "Cost cannot be negative."
            });
        }

        var created = _service.AddRepair(repair);

        return Created("", created);
    }
  
    [HttpGet("usage")]
    public IActionResult Usage()
    {
        var key = Request.Headers["X-Api-Key"].ToString();

        if (string.IsNullOrWhiteSpace(key))
            key = "NO_KEY";

        if (!_usageCounts.ContainsKey(key))
            _usageCounts[key] = 0;

        _usageCounts[key]++;

        return Ok(new
        {
            clientId = key,
            callCount = _usageCounts[key]
        });
    }


}

