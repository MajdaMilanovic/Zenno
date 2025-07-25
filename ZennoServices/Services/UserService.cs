using Microsoft.EntityFrameworkCore;
using Zenno.Model;
using Zenno.Model.SearchObjects;
using ZennoServices.Database;
using ZennoServices.Interfaces;

namespace ZennoServices.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Reservations)
                .Include(u => u.Reviews)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Reservations)
                .Include(u => u.Reviews)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Reservations)
                .Include(u => u.Reviews)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> CreateAsync(User user)
        {
            // In a real application, you would hash the password here
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateAsync(int id, User user)
        {
            var existingUser = await _context.Users.FindAsync(id);
            
            if (existingUser == null)
                return null;

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            // Only update password if it's provided
            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                // In a real application, you would hash the password here
                existingUser.PasswordHash = user.PasswordHash;
            }
            existingUser.Role = user.Role;

            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<User>> SearchAsync(UserSearchObject searchObject)
        {
            var query = _context.Users
                .Include(u => u.Reservations)
                .Include(u => u.Reviews)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchObject.SearchTerm))
            {
                query = query.Where(u => 
                    u.Username.Contains(searchObject.SearchTerm) || 
                    u.Email.Contains(searchObject.SearchTerm));
            }

            if (!string.IsNullOrWhiteSpace(searchObject.Role))
            {
                query = query.Where(u => u.Role == searchObject.Role);
            }

            if (searchObject.HasActiveReservations.HasValue)
            {
                if (searchObject.HasActiveReservations.Value)
                {
                    query = query.Where(u => u.Reservations.Any(r => 
                        r.Status == ReservationStatus.Confirmed || 
                        r.Status == ReservationStatus.Pending));
                }
                else
                {
                    query = query.Where(u => !u.Reservations.Any(r => 
                        r.Status == ReservationStatus.Confirmed || 
                        r.Status == ReservationStatus.Pending));
                }
            }

            return await query.ToListAsync();
        }

        public async Task<List<Reservation>> GetUserReservationsAsync(int userId)
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CheckInDate)
                .ToListAsync();
        }

        public async Task<List<Review>> GetUserReviewsAsync(int userId)
        {
            return await _context.Reviews
                .Include(r => r.Room)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.DatePosted)
                .ToListAsync();
        }
    }
} 