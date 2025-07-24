using Zenno.Model;
using Zenno.Model.SearchObjects;

namespace ZennoServices.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task<User?> UpdateAsync(int id, User user);
        Task DeleteAsync(int id);
        Task<List<User>> SearchAsync(UserSearchObject searchObject);
        Task<List<Reservation>> GetUserReservationsAsync(int userId);
        Task<List<Review>> GetUserReviewsAsync(int userId);
    }
} 