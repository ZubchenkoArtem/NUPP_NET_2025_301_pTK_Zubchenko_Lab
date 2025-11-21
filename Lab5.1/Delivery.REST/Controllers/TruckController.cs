using Delivery.Common;
using Delivery.Common.Services;
using Delivery.REST.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Delivery.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TruckController : ControllerBase
    {
        private readonly CrudServiceAsync<Truck> _service;

        public TruckController()
        {
            _service = new CrudServiceAsync<Truck>("trucks.json");
        }

        [HttpGet]
        public async Task<IEnumerable<Truck>> GetAll() => await _service.ReadAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Truck>> Get(Guid id)
        {
            var truck = await _service.ReadAsync(id);
            if (truck == null) return NotFound();
            return Ok(truck);
        }

        [HttpPost]
        public async Task<ActionResult> Create(TruckModel model)
        {
            var truck = new Truck
            {
                LicensePlate = model.LicensePlate,
                MaxLoadKg = model.MaxLoadKg
            };

            await _service.CreateAsync(truck);
            await _service.SaveAsync();
            return CreatedAtAction(nameof(Get), new { id = truck.Id }, truck);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var truck = await _service.ReadAsync(id);
            if (truck == null) return NotFound();

            await _service.RemoveAsync(truck);
            await _service.SaveAsync();
            return NoContent();
        }
    }
}
