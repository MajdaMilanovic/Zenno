using Microsoft.EntityFrameworkCore;
using Zenno.Model;
using Zenno.Model.SearchObjects;
using ZennoServices.Database;
using ZennoServices.Interfaces;

namespace ZennoServices.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationDbContext _context;

        public RoomService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Room>> GetAllAsync()
        {
            return await _context.Rooms
                .Include(r => r.Images)
                .Include(r => r.Reviews)
                .ToListAsync();
        }

        public async Task<Room?> GetByIdAsync(int id)
        {
            return await _context.Rooms
                .Include(r => r.Images)
                .Include(r => r.Reviews)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Room> CreateAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<Room?> UpdateAsync(int id, Room room)
        {
            var existingRoom = await _context.Rooms.FindAsync(id);
            
            if (existingRoom == null)
                return null;

            existingRoom.Name = room.Name;
            existingRoom.Description = room.Description;
            existingRoom.PricePerNight = room.PricePerNight;
            existingRoom.Capacity = room.Capacity;
            existingRoom.Type = room.Type;
            existingRoom.IsAvailable = room.IsAvailable;

            await _context.SaveChangesAsync();
            return existingRoom;
        }

        public async Task DeleteAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Room>> SearchAsync(RoomSearchObject searchObject)
        {
            var query = _context.Rooms
                .Include(r => r.Images)
                .Include(r => r.Reviews)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchObject.SearchTerm))
            {
                query = query.Where(r => 
                    r.Name.Contains(searchObject.SearchTerm) || 
                    r.Description.Contains(searchObject.SearchTerm) ||
                    r.Type.Contains(searchObject.SearchTerm));
            }

            if (searchObject.MinPrice.HasValue)
                query = query.Where(r => r.PricePerNight >= searchObject.MinPrice.Value);

            if (searchObject.MaxPrice.HasValue)
                query = query.Where(r => r.PricePerNight <= searchObject.MaxPrice.Value);

            if (searchObject.MinCapacity.HasValue)
                query = query.Where(r => r.Capacity >= searchObject.MinCapacity.Value);

            if (searchObject.MaxCapacity.HasValue)
                query = query.Where(r => r.Capacity <= searchObject.MaxCapacity.Value);

            if (!string.IsNullOrWhiteSpace(searchObject.Type))
                query = query.Where(r => r.Type == searchObject.Type);

            if (searchObject.IsAvailable.HasValue)
                query = query.Where(r => r.IsAvailable == searchObject.IsAvailable.Value);

            if (searchObject.CheckInDate.HasValue && searchObject.CheckOutDate.HasValue)
            {
                query = query.Where(r => !r.Reservations.Any(res =>
                    (res.CheckInDate <= searchObject.CheckOutDate && res.CheckOutDate >= searchObject.CheckInDate) &&
                    res.Status != ReservationStatus.Cancelled));
            }

            return await query.ToListAsync();
        }

        public async Task<List<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut, int? capacity = null)
        {
            var query = _context.Rooms
                .Include(r => r.Images)
                .Include(r => r.Reviews)
                .Where(r => r.IsAvailable &&
                    !r.Reservations.Any(res =>
                        (res.CheckInDate <= checkOut && res.CheckOutDate >= checkIn) &&
                        res.Status != ReservationStatus.Cancelled));

            if (capacity.HasValue)
                query = query.Where(r => r.Capacity >= capacity.Value);

            return await query.ToListAsync();
        }

        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var room = await _context.Rooms
                .Include(r => r.Reservations)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null || !room.IsAvailable)
                return false;

            return !room.Reservations.Any(res =>
                (res.CheckInDate <= checkOut && res.CheckOutDate >= checkIn) &&
                res.Status != ReservationStatus.Cancelled);
        }

        public async Task AddRoomImageAsync(int roomId, RoomImage image)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
                throw new ArgumentException("Room not found", nameof(roomId));

            image.RoomId = roomId;
            _context.RoomImages.Add(image);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoomImageAsync(int roomId, int imageId)
        {
            var image = await _context.RoomImages
                .FirstOrDefaultAsync(i => i.Id == imageId && i.RoomId == roomId);

            if (image != null)
            {
                _context.RoomImages.Remove(image);
                await _context.SaveChangesAsync();
            }
        }
    }
} 