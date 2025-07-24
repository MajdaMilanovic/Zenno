using Microsoft.EntityFrameworkCore;
using Zenno.Model;
using Zenno.Model.SearchObjects;
using ZennoServices.Database;
using ZennoServices.Interfaces;

namespace ZennoServices.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Review>> GetAllAsync()
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Room)
                .OrderByDescending(r => r.DatePosted)
                .ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Room)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Review> CreateAsync(Review review)
        {
            review.DatePosted = DateTime.UtcNow;
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<Review?> UpdateAsync(int id, Review review)
        {
            var existingReview = await _context.Reviews.FindAsync(id);
            
            if (existingReview == null)
                return null;

            existingReview.Rating = review.Rating;
            existingReview.Comment = review.Comment;
            // Don't update DatePosted, UserId, or RoomId as these shouldn't change

            await _context.SaveChangesAsync();
            return existingReview;
        }

        public async Task DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Review>> SearchAsync(ReviewSearchObject searchObject)
        {
            var query = _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Room)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchObject.SearchTerm))
            {
                query = query.Where(r => 
                    r.Comment != null && 
                    r.Comment.Contains(searchObject.SearchTerm));
            }

            if (searchObject.MinRating.HasValue)
                query = query.Where(r => r.Rating >= searchObject.MinRating.Value);

            if (searchObject.MaxRating.HasValue)
                query = query.Where(r => r.Rating <= searchObject.MaxRating.Value);

            if (searchObject.FromDate.HasValue)
                query = query.Where(r => r.DatePosted >= searchObject.FromDate.Value);

            if (searchObject.ToDate.HasValue)
                query = query.Where(r => r.DatePosted <= searchObject.ToDate.Value);

            if (searchObject.UserId.HasValue)
                query = query.Where(r => r.UserId == searchObject.UserId.Value);

            if (searchObject.RoomId.HasValue)
                query = query.Where(r => r.RoomId == searchObject.RoomId.Value);

            return await query
                .OrderByDescending(r => r.DatePosted)
                .ToListAsync();
        }

        public async Task<List<Review>> GetRoomReviewsAsync(int roomId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.RoomId == roomId)
                .OrderByDescending(r => r.DatePosted)
                .ToListAsync();
        }

        public async Task<double> GetRoomAverageRatingAsync(int roomId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.RoomId == roomId)
                .ToListAsync();

            if (!reviews.Any())
                return 0;

            return reviews.Average(r => r.Rating);
        }
    }
} 