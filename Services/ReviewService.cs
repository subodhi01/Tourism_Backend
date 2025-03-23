using TourismGalle.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TourismGalle.Services
{
    public interface IReviewService
    {
        Task<List<Review>> GetAllReviews();
        Task<Review> GetReviewById(int id);
        Task<Review> CreateReview(Review review);
        Task<Review> UpdateReview(int id, Review review);
        Task<bool> DeleteReview(int id);
        Task<List<Review>> GetReviewsByItemId(int itemId);
    }

    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Review>> GetAllReviews()
        {
            try
            {
                return await _context.Reviews.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                throw new Exception("Error fetching reviews", ex);
            }
        }

        public async Task<Review> GetReviewById(int id)
        {
            try
            {
                return await _context.Reviews.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching review by ID", ex);
            }
        }

        public async Task<Review> CreateReview(Review review)
        {
            try
            {
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
                return review;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating review", ex);
            }
        }

        public async Task<Review> UpdateReview(int id, Review review)
        {
            try
            {
                var existingReview = await _context.Reviews.FindAsync(id);
                if (existingReview == null)
                {
                    return null;
                }

                // Update all fields
                existingReview.Comment = review.Comment;
                existingReview.Email = review.Email;
                existingReview.Type = review.Type;
                existingReview.Rating = review.Rating;
                existingReview.Date = review.Date;

                await _context.SaveChangesAsync();
                return existingReview;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating review", ex);
            }
        }

        public async Task<bool> DeleteReview(int id)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(id);
                if (review == null)
                {
                    return false;
                }

                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting review", ex);
            }
        }

        public async Task<List<Review>> GetReviewsByItemId(int itemId)
        {
            try
            {
                return await _context.Reviews
                                    .Where(r => r.ItemId == itemId)
                                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching reviews by itemId", ex);
            }
        }
    }
}