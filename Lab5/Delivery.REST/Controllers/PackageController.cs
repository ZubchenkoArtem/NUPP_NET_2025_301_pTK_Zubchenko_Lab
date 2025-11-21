using Microsoft.AspNetCore.Mvc;
using Delivery.Common;
using Delivery.Common.Services;
using Delivery.REST.Models;

namespace Delivery.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackageController : ControllerBase
    {
        private readonly CrudServiceAsync<Package> _service;

        public PackageController()
        {
            _service = new CrudServiceAsync<Package>("packages.json");
        }

        [HttpGet]
        public async Task<IEnumerable<Package>> GetAll() => await _service.ReadAllAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Package>> Get(Guid id)
        {
            var pkg = await _service.ReadAsync(id);
            if (pkg == null) return NotFound();
            return Ok(pkg);
        }

        [HttpPost]
        public async Task<ActionResult> Create(PackageModel model)
        {
            var pkg = new Package(model.DestinationAddress, model.WeightKg)
            {
                IsFragile = model.IsFragile
            };

            await _service.CreateAsync(pkg);
            await _service.SaveAsync();
            return CreatedAtAction(nameof(Get), new { id = pkg.Id }, pkg);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var pkg = await _service.ReadAsync(id);
            if (pkg == null) return NotFound();

            await _service.RemoveAsync(pkg);
            await _service.SaveAsync();
            return NoContent();
        }
    }
}
