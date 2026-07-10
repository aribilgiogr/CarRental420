using Core.Concretes.DTOs.Review;
using Core.Concretes.Entities;
using Core.Utils;

namespace Business.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ReviewResponseDTO> CreateAsync(CreateReviewDTO dto)
        {
            var entity = new Review
            {
                RentalId = dto.RentalId,
                MemberId = dto.MemberId,
                Rating = dto.Rating,
                Title = dto.Title,
                Comment = dto.Comment,
                VehicleRating = dto.VehicleRating,
                ServiceRating = dto.ServiceRating,
                CleanlinessRating = dto.CleanlinessRating,
                StaffRating = dto.StaffRating,
                IsVerified = dto.IsVerified,
                IsPublished = dto.IsPublished,
                ReviewDate = dto.ReviewDate,
                VehicleId = dto.VehicleId,
                Active = true,
                Deleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Review>().InsertOneAsync(entity);
            await _unitOfWork.CommitAsync();

            return MapToResponseDTO(entity);
        }

        public async Task UpdateAsync(UpdateReviewDTO dto)
        {
            var entity = await _unitOfWork.Repository<Review>().FindByIdAsync(dto.Id);
            if (entity == null || entity.Deleted) return;

            entity.RentalId = dto.RentalId;
            entity.MemberId = dto.MemberId;
            entity.Rating = dto.Rating;
            entity.Title = dto.Title;
            entity.Comment = dto.Comment;
            entity.VehicleRating = dto.VehicleRating;
            entity.ServiceRating = dto.ServiceRating;
            entity.CleanlinessRating = dto.CleanlinessRating;
            entity.StaffRating = dto.StaffRating;
            entity.IsVerified = dto.IsVerified;
            entity.IsPublished = dto.IsPublished;
            entity.ReviewDate = dto.ReviewDate;
            entity.VehicleId = dto.VehicleId;
            entity.Active = dto.Active;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Review>().UpdateOneAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Review>().FindByIdAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                entity.Active = false;
                entity.UpdatedAt = DateTime.UtcNow;
                
                await _unitOfWork.Repository<Review>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<bool> ReviewExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Review>().AnyAsync(x => x.Id == id && !x.Deleted);
        }

        public async Task<IEnumerable<ReviewResponseDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.Repository<Review>().FindManyAsync(x => !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<ReviewResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Review>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return MapToResponseDTO(entity);
        }

        public async Task<IEnumerable<ReviewResponseDTO>> GetByMemberAsync(string memberId)
        {
            var items = await _unitOfWork.Repository<Review>().FindManyAsync(x => x.MemberId == memberId && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<ReviewResponseDTO>> GetByVehicleAsync(int vehicleId)
        {
            var items = await _unitOfWork.Repository<Review>().FindManyAsync(x => x.VehicleId == vehicleId && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<ReviewResponseDTO>> GetPublishedReviewsAsync()
        {
            var items = await _unitOfWork.Repository<Review>().FindManyAsync(x => x.IsPublished && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<ReviewResponseDTO>> GetUnpublishedReviewsAsync()
        {
            var items = await _unitOfWork.Repository<Review>().FindManyAsync(x => !x.IsPublished && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<ReviewResponseDTO>> GetVerifiedReviewsAsync()
        {
            var items = await _unitOfWork.Repository<Review>().FindManyAsync(x => x.IsVerified && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<double> GetAverageRatingAsync()
        {
            var items = await _unitOfWork.Repository<Review>().FindManyAsync(x => x.IsPublished && !x.Deleted);
            var reviews = items.ToList();
            if (!reviews.Any()) return 0;
            return reviews.Average(x => x.Rating);
        }

        public async Task<double> GetAverageRatingByVehicleAsync(int vehicleId)
        {
            var items = await _unitOfWork.Repository<Review>().FindManyAsync(x => x.VehicleId == vehicleId && x.IsPublished && !x.Deleted);
            var reviews = items.ToList();
            if (!reviews.Any()) return 0;
            return reviews.Average(x => x.Rating);
        }

        public async Task PublishReviewAsync(int reviewId)
        {
            var entity = await _unitOfWork.Repository<Review>().FindByIdAsync(reviewId);
            if (entity != null && !entity.Deleted)
            {
                entity.IsPublished = true;
                entity.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Repository<Review>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task UnpublishReviewAsync(int reviewId)
        {
            var entity = await _unitOfWork.Repository<Review>().FindByIdAsync(reviewId);
            if (entity != null && !entity.Deleted)
            {
                entity.IsPublished = false;
                entity.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Repository<Review>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task VerifyReviewAsync(int reviewId)
        {
            var entity = await _unitOfWork.Repository<Review>().FindByIdAsync(reviewId);
            if (entity != null && !entity.Deleted)
            {
                entity.IsVerified = true;
                entity.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Repository<Review>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<IEnumerable<ReviewResponseDTO>> GetHighRatedReviewsAsync(int rating = 4)
        {
            var items = await _unitOfWork.Repository<Review>().FindManyAsync(x => x.Rating >= rating && x.IsPublished && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<ReviewResponseDTO>> GetLowRatedReviewsAsync(int rating = 2)
        {
            var items = await _unitOfWork.Repository<Review>().FindManyAsync(x => x.Rating <= rating && x.IsPublished && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        private static ReviewResponseDTO MapToResponseDTO(Review entity)
        {
            return new ReviewResponseDTO
            {
                Id = entity.Id,
                RentalId = entity.RentalId,
                MemberId = entity.MemberId,
                Rating = entity.Rating,
                Title = entity.Title,
                Comment = entity.Comment,
                VehicleRating = entity.VehicleRating,
                ServiceRating = entity.ServiceRating,
                CleanlinessRating = entity.CleanlinessRating,
                StaffRating = entity.StaffRating,
                IsVerified = entity.IsVerified,
                IsPublished = entity.IsPublished,
                ReviewDate = entity.ReviewDate,
                VehicleId = entity.VehicleId,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Active = entity.Active
            };
        }
    }
}