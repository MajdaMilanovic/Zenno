using Microsoft.EntityFrameworkCore;
using Zenno.Model;
using Zenno.Model.SearchObjects;
using ZennoServices.Database;
using ZennoServices.Interfaces;

namespace ZennoServices.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _context;

        public ReservationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Reservation>> GetAllAsync()
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Room)
                .ToListAsync();
        }

        public async Task<Reservation> GetByIdAsync(int id)
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Reservation> CreateAsync(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task<Reservation> UpdateAsync(int id, Reservation reservation)
        {
            var existingReservation = await _context.Reservations.FindAsync(id);
            
            if (existingReservation == null)
                return null;

            existingReservation.CheckInDate = reservation.CheckInDate;
            existingReservation.CheckOutDate = reservation.CheckOutDate;
            existingReservation.TotalPrice = reservation.TotalPrice;
            existingReservation.Status = reservation.Status;
            existingReservation.UserId = reservation.UserId;
            existingReservation.RoomId = reservation.RoomId;

            await _context.SaveChangesAsync();
            return existingReservation;
        }

        public async Task DeleteAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Reservation>> SearchAsync(ReservationSearchObject searchObject)
        {
            var query = _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Room)
                .AsQueryable();

            if (searchObject.UserId.HasValue)
                query = query.Where(r => r.UserId == searchObject.UserId);

            if (searchObject.RoomId.HasValue)
                query = query.Where(r => r.RoomId == searchObject.RoomId);

            if (searchObject.FromDate.HasValue)
                query = query.Where(r => r.CheckInDate >= searchObject.FromDate);

            if (searchObject.ToDate.HasValue)
                query = query.Where(r => r.CheckOutDate <= searchObject.ToDate);

            if (searchObject.Status.HasValue)
                query = query.Where(r => r.Status == searchObject.Status);

            return await query.ToListAsync();
        }
    }
} 