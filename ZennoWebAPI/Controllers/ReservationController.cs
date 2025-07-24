using Microsoft.AspNetCore.Mvc;
using Zenno.Model;
using Zenno.Model.SearchObjects;
using ZennoServices.Interfaces;

namespace ZennoWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Reservation>>> GetAll()
        {
            return await _reservationService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetById(int id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);
            if (reservation == null)
                return NotFound();

            return reservation;
        }

        [HttpPost]
        public async Task<ActionResult<Reservation>> Create(Reservation reservation)
        {
            var createdReservation = await _reservationService.CreateAsync(reservation);
            return CreatedAtAction(nameof(GetById), new { id = createdReservation.Id }, createdReservation);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Reservation>> Update(int id, Reservation reservation)
        {
            var updatedReservation = await _reservationService.UpdateAsync(id, reservation);
            if (updatedReservation == null)
                return NotFound();

            return updatedReservation;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _reservationService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Reservation>>> Search([FromQuery] ReservationSearchObject searchObject)
        {
            return await _reservationService.SearchAsync(searchObject);
        }
    }
} 