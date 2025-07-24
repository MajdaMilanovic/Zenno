using Zenno.Model;
using Zenno.Model.SearchObjects;

namespace ZennoServices.Interfaces
{
    public interface IReviewService
    {
        Task<List<Review>> GetAllAsync();
        Task<Review?> GetByIdAsync(int id);
        Task<Review> CreateAsync(Review review);
        Task<Review?> UpdateAsync(int id, Review review);
        Task DeleteAsync(int id);
        Task<List<Review>> SearchAsync(ReviewSearchObject searchObject);
        Task<List<Review>> GetRoomReviewsAsync(int roomId);
        Task<double> GetRoomAverageRatingAsync(int roomId);
    }
} 