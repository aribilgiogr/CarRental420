using Core.Concretes.DTOs.Review;

namespace Business.Services
{
    public interface IReviewService
    {
        Task<ReviewResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<ReviewResponseDTO>> GetAllAsync();
        Task<IEnumerable<ReviewResponseDTO>> GetByMemberAsync(string memberId);
        Task<IEnumerable<ReviewResponseDTO>> GetByVehicleAsync(int vehicleId);
        Task<IEnumerable<ReviewResponseDTO>> GetPublishedReviewsAsync();
        Task<IEnumerable<ReviewResponseDTO>> GetUnpublishedReviewsAsync();
        Task<IEnumerable<ReviewResponseDTO>> GetVerifiedReviewsAsync();
        Task<double> GetAverageRatingAsync();
        Task<double> GetAverageRatingByVehicleAsync(int vehicleId);
        Task<ReviewResponseDTO> CreateAsync(CreateReviewDTO dto);
        Task UpdateAsync(UpdateReviewDTO dto);
        Task DeleteAsync(int id);
        Task<bool> ReviewExistsAsync(int id);
        Task PublishReviewAsync(int reviewId);
        Task UnpublishReviewAsync(int reviewId);
        Task VerifyReviewAsync(int reviewId);
        Task<IEnumerable<ReviewResponseDTO>> GetHighRatedReviewsAsync(int rating = 4);
        Task<IEnumerable<ReviewResponseDTO>> GetLowRatedReviewsAsync(int rating = 2);
    }
}
