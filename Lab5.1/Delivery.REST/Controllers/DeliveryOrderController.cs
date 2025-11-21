using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Delivery.Common;
using Delivery.Common.Services;
using Delivery.REST.Models;

namespace Delivery.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // ✅ Додаємо авторизацію на весь контролер
    public class DeliveryOrderController : ControllerBase
    {
        private readonly CrudServiceAsync<DeliveryOrder> _service;
        private readonly CrudServiceAsync<Package> _packageService;
        private readonly CrudServiceAsync<Truck> _truckService;

        public DeliveryOrderController()
        {
            _service = new CrudServiceAsync<DeliveryOrder>("orders.json");
            _packageService = new CrudServiceAsync<Package>("packages.json");
            _truckService = new CrudServiceAsync<Truck>("trucks.json");
        }

        // GET: api/deliveryorder
        [HttpGet]
        public async Task<IEnumerable<DeliveryOrder>> GetAll() => await _service.ReadAllAsync();

        // GET: api/deliveryorder/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryOrder>> Get(Guid id)
        {
            var order = await _service.ReadAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        // POST: api/deliveryorder
        [Authorize(Roles = "User,Manager,Admin")] // тільки авторизовані користувачі
        [HttpPost]
        public async Task<ActionResult> Create(DeliveryOrderModel model)
        {
            var package = await _packageService.ReadAsync(model.PackageId);
            var truck = await _truckService.ReadAsync(model.VehicleId);

            if (package == null) return BadRequest("Package not found.");
            if (truck == null) return BadRequest("Truck not found.");

            var order = new DeliveryOrder(package, truck)
            {
                Status = model.Status ?? "Pending"
            };

            await _service.CreateAsync(order);
            await _service.SaveAsync();

            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }

        // PUT: api/deliveryorder/{id}/status
        [Authorize(Roles = "Manager,Admin")] // тільки менеджери та адміни
        [HttpPut("{id}/status")]
        public async Task<ActionResult> UpdateStatus(Guid id, [FromBody] string newStatus)
        {
            var order = await _service.ReadAsync(id);
            if (order == null) return NotFound();

            order.UpdateStatus(newStatus);
            await _service.UpdateAsync(order);
            await _service.SaveAsync();

            return Ok(order);
        }

        // DELETE: api/deliveryorder/{id}
        [Authorize(Roles = "Admin")] // тільки адмін
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var order = await _service.ReadAsync(id);
            if (order == null) return NotFound();

            await _service.RemoveAsync(order);
            await _service.SaveAsync();

            return NoContent();
        }
    }
}
