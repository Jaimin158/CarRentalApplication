using Microsoft.AspNetCore.Mvc;
using VehicleInventory.Application.DTOs;
using VehicleInventory.Application.Services;

namespace VehicleInventory.WebAPI.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    public class VehiclesController : ControllerBase
    {

        private readonly VehicleService _service;

        public VehiclesController(VehicleService service)
        {
            _service = service;
        }

        // GET: api/vehicles
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<VehicleDto>>> GetAll(CancellationToken ct)
        {
            var vehicles = await _service.GetAllVehiclesAsync(ct);
            return Ok(vehicles);
        }

        // GET: api/vehicles/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<VehicleDto>> GetById(Guid id, CancellationToken ct)
        {
            var vehicle = await _service.GetVehicleByIdAsync(id, ct);

            // If we didn't find a vehicle, return 404
            if (vehicle == null)
                return NotFound();

            return Ok(vehicle);
        }

        // POST: api/vehicles
        [HttpPost]
        public async Task<ActionResult<VehicleDto>> Create([FromBody] CreateVehicleRequest request, CancellationToken ct)
        {
            var created = await _service.CreateVehicleAsync(request, ct);

            // Return 201 and where to find it (GetById)
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/vehicles/{id}/status
        [HttpPut("{id:guid}/status")]
        public async Task<ActionResult<VehicleDto>> UpdateStatus(
            Guid id,
            [FromBody] UpdateVehicleStatusRequest request,
            CancellationToken ct)
        {
            var updated = await _service.UpdateVehicleStatusAsync(id, request, ct);

            // If the vehicle doesn't exist, return 404
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        // DELETE: api/vehicles/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var deleted = await _service.DeleteVehicleAsync(id, ct);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}

