using Microsoft.AspNetCore.Mvc;
using Zenno.Model;
using Zenno.Model.SearchObjects;
using ZennoServices.Interfaces;

namespace ZennoWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Service>>> GetAll()
        {
            return await _serviceService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetById(int id)
        {
            var service = await _serviceService.GetByIdAsync(id);
            if (service == null)
                return NotFound();

            return service;
        }

        [HttpPost]
        public async Task<ActionResult<Service>> Create(Service service)
        {
            var createdService = await _serviceService.CreateAsync(service);
            return CreatedAtAction(nameof(GetById), new { id = createdService.Id }, createdService);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Service>> Update(int id, Service service)
        {
            var updatedService = await _serviceService.UpdateAsync(id, service);
            if (updatedService == null)
                return NotFound();

            return updatedService;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _serviceService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Service>>> Search([FromQuery] ServiceSearchObject searchObject)
        {
            return await _serviceService.SearchAsync(searchObject);
        }

        [HttpGet("reservation/{reservationId}")]
        public async Task<ActionResult<List<Service>>> GetReservationServices(int reservationId)
        {
            return await _serviceService.GetReservationServicesAsync(reservationId);
        }

        [HttpPost("reservation/{reservationId}/service/{serviceId}")]
        public async Task<ActionResult> AddServiceToReservation(int reservationId, int serviceId, [FromQuery] int quantity)
        {
            try
            {
                await _serviceService.AddServiceToReservationAsync(reservationId, serviceId, quantity);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("reservation/{reservationId}/service/{serviceId}")]
        public async Task<ActionResult> RemoveServiceFromReservation(int reservationId, int serviceId)
        {
            await _serviceService.RemoveServiceFromReservationAsync(reservationId, serviceId);
            return NoContent();
        }
    }
} 