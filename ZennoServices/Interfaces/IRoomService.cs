using Zenno.Model;
using Zenno.Model.SearchObjects;

namespace ZennoServices.Interfaces
{
    public interface IRoomService
    {
        Task<List<Room>> GetAllAsync();
        Task<Room?> GetByIdAsync(int id);
        Task<Room> CreateAsync(Room room);
        Task<Room?> UpdateAsync(int id, Room room);
        Task DeleteAsync(int id);
        Task<List<Room>> SearchAsync(RoomSearchObject searchObject);
        Task<List<Room>> GetAvailableRoomsAsync(DateTime checkIn, DateTime checkOut, int? capacity = null);
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime checkIn, DateTime checkOut);
        Task AddRoomImageAsync(int roomId, RoomImage image);
        Task DeleteRoomImageAsync(int roomId, int imageId);
    }
} 