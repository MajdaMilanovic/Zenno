using Microsoft.AspNetCore.Mvc;
using Zenno.Model;
using Zenno.Model.SearchObjects;
using ZennoServices.Interfaces;

namespace ZennoWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Room>>> GetAll()
        {
            return await _roomService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetById(int id)
        {
            var room = await _roomService.GetByIdAsync(id);
            if (room == null)
                return NotFound();

            return room;
        }

        [HttpPost]
        public async Task<ActionResult<Room>> Create(Room room)
        {
            var createdRoom = await _roomService.CreateAsync(room);
            return CreatedAtAction(nameof(GetById), new { id = createdRoom.Id }, createdRoom);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Room>> Update(int id, Room room)
        {
            var updatedRoom = await _roomService.UpdateAsync(id, room);
            if (updatedRoom == null)
                return NotFound();

            return updatedRoom;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _roomService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Room>>> Search([FromQuery] RoomSearchObject searchObject)
        {
            return await _roomService.SearchAsync(searchObject);
        }

        [HttpGet("available")]
        public async Task<ActionResult<List<Room>>> GetAvailableRooms(
            [FromQuery] DateTime checkIn,
            [FromQuery] DateTime checkOut,
            [FromQuery] int? capacity)
        {
            return await _roomService.GetAvailableRoomsAsync(checkIn, checkOut, capacity);
        }

        [HttpGet("{id}/availability")]
        public async Task<ActionResult<bool>> CheckAvailability(
            int id,
            [FromQuery] DateTime checkIn,
            [FromQuery] DateTime checkOut)
        {
            return await _roomService.IsRoomAvailableAsync(id, checkIn, checkOut);
        }

        [HttpPost("{roomId}/images")]
        public async Task<ActionResult> AddImage(int roomId, RoomImage image)
        {
            try
            {
                await _roomService.AddRoomImageAsync(roomId, image);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{roomId}/images/{imageId}")]
        public async Task<ActionResult> DeleteImage(int roomId, int imageId)
        {
            await _roomService.DeleteRoomImageAsync(roomId, imageId);
            return NoContent();
        }
    }
} 