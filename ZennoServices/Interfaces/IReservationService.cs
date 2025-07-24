using Zenno.Model;
using Zenno.Model.SearchObjects;

namespace ZennoServices.Interfaces
{
    public interface IReservationService
    {
        Task<List<Reservation>> GetAllAsync();
        Task<Reservation> GetByIdAsync(int id);
        Task<Reservation> CreateAsync(Reservation reservation);
        Task<Reservation> UpdateAsync(int id, Reservation reservation);
        Task DeleteAsync(int id);
        Task<List<Reservation>> SearchAsync(ReservationSearchObject searchObject);
    }
} 