using Microsoft.AspNetCore.Mvc;
using Zenno.Model;
using Zenno.Model.SearchObjects;
using ZennoServices.Interfaces;

namespace ZennoWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Review>>> GetAll()
        {
            return await _reviewService.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetById(int id)
        {
            var review = await _reviewService.GetByIdAsync(id);
            if (review == null)
                return NotFound();

            return review;
        }

        [HttpPost]
        public async Task<ActionResult<Review>> Create(Review review)
        {
            var createdReview = await _reviewService.CreateAsync(review);
            return CreatedAtAction(nameof(GetById), new { id = createdReview.Id }, createdReview);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Review>> Update(int id, Review review)
        {
            var updatedReview = await _reviewService.UpdateAsync(id, review);
            if (updatedReview == null)
                return NotFound();

            return updatedReview;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _reviewService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<Review>>> Search([FromQuery] ReviewSearchObject searchObject)
        {
            return await _reviewService.SearchAsync(searchObject);
        }

        [HttpGet("room/{roomId}")]
        public async Task<ActionResult<List<Review>>> GetRoomReviews(int roomId)
        {
            return await _reviewService.GetRoomReviewsAsync(roomId);
        }

        [HttpGet("room/{roomId}/rating")]
        public async Task<ActionResult<double>> GetRoomAverageRating(int roomId)
        {
            return await _reviewService.GetRoomAverageRatingAsync(roomId);
        }
    }
} 