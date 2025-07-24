using Microsoft.EntityFrameworkCore;
using Zenno.Model;
using Zenno.Model.SearchObjects;
using ZennoServices.Database;
using ZennoServices.Interfaces;

namespace ZennoServices.Services
{
    public class ServiceService : IServiceService
    {
        private readonly ApplicationDbContext _context;

        public ServiceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Service>> GetAllAsync()
        {
            return await _context.Services
                .Include(s => s.ReservationServices)
                .ToListAsync();
        }

        public async Task<Service?> GetByIdAsync(int id)
        {
            return await _context.Services
                .Include(s => s.ReservationServices)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Service> CreateAsync(Service service)
        {
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task<Service?> UpdateAsync(int id, Service service)
        {
            var existingService = await _context.Services.FindAsync(id);
            
            if (existingService == null)
                return null;

            existingService.Name = service.Name;
            existingService.Price = service.Price;

            await _context.SaveChangesAsync();
            return existingService;
        }

        public async Task DeleteAsync(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service != null)
            {
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Service>> SearchAsync(ServiceSearchObject searchObject)
        {
            var query = _context.Services
                .Include(s => s.ReservationServices)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchObject.SearchTerm))
            {
                query = query.Where(s => 
                    s.Name.Contains(searchObject.SearchTerm));
            }

            if (searchObject.MinPrice.HasValue)
                query = query.Where(s => s.Price >= searchObject.MinPrice.Value);

            if (searchObject.MaxPrice.HasValue)
                query = query.Where(s => s.Price <= searchObject.MaxPrice.Value);

            return await query.ToListAsync();
        }

        public async Task<List<Service>> GetReservationServicesAsync(int reservationId)
        {
            var reservationServices = await _context.ReservationServices
                .Include(rs => rs.Service)
                .Where(rs => rs.ReservationId == reservationId)
                .ToListAsync();

            return reservationServices.Select(rs => rs.Service).ToList();
        }

        public async Task AddServiceToReservationAsync(int reservationId, int serviceId, int quantity)
        {
            var reservation = await _context.Reservations.FindAsync(reservationId);
            if (reservation == null)
                throw new ArgumentException("Reservation not found", nameof(reservationId));

            var service = await _context.Services.FindAsync(serviceId);
            if (service == null)
                throw new ArgumentException("Service not found", nameof(serviceId));

            var reservationService = new ReservationServiceMapping
            {
                ReservationId = reservationId,
                ServiceId = serviceId,
                Quantity = quantity,
                Price = service.Price * quantity
            };

            _context.ReservationServices.Add(reservationService);
            await _context.SaveChangesAsync();

            // Update reservation total price
            reservation.TotalPrice += reservationService.Price;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveServiceFromReservationAsync(int reservationId, int serviceId)
        {
            var reservationService = await _context.ReservationServices
                .FirstOrDefaultAsync(rs => rs.ReservationId == reservationId && rs.ServiceId == serviceId);

            if (reservationService != null)
            {
                var reservation = await _context.Reservations.FindAsync(reservationId);
                if (reservation != null)
                {
                    // Update reservation total price
                    reservation.TotalPrice -= reservationService.Price;
                }

                _context.ReservationServices.Remove(reservationService);
                await _context.SaveChangesAsync();
            }
        }
    }
} 