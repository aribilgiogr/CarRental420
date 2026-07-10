using Core.Concretes.DTOs.Penalty;
using Core.Concretes.Entities;
using Core.Utils;

namespace Business.Services
{
    public class PenaltyService : IPenaltyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PenaltyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PenaltyResponseDTO> CreateAsync(CreatePenaltyDTO dto)
        {
            var entity = new Penalty
            {
                RentalId = dto.RentalId,
                MemberId = dto.MemberId,
                VehicleId = dto.VehicleId,
                PenaltyType = dto.PenaltyType,
                Description = dto.Description,
                PenaltyAmount = dto.PenaltyAmount,
                PenaltyStatus = dto.PenaltyStatus ?? "Beklemede",
                ApprovedDate = dto.ApprovedDate,
                ApprovedBy = dto.ApprovedBy,
                PaidDate = dto.PaidDate,
                PhotosPath = dto.PhotosPath,
                Notes = dto.Notes,
                Active = true,
                Deleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Penalty>().InsertOneAsync(entity);
            await _unitOfWork.CommitAsync();

            return MapToResponseDTO(entity);
        }

        public async Task UpdateAsync(UpdatePenaltyDTO dto)
        {
            var entity = await _unitOfWork.Repository<Penalty>().FindByIdAsync(dto.Id);
            if (entity == null || entity.Deleted) return;

            entity.RentalId = dto.RentalId;
            entity.MemberId = dto.MemberId;
            entity.VehicleId = dto.VehicleId;
            entity.PenaltyType = dto.PenaltyType;
            entity.Description = dto.Description;
            entity.PenaltyAmount = dto.PenaltyAmount;
            entity.PenaltyStatus = dto.PenaltyStatus;
            entity.ApprovedDate = dto.ApprovedDate;
            entity.ApprovedBy = dto.ApprovedBy;
            entity.PaidDate = dto.PaidDate;
            entity.PhotosPath = dto.PhotosPath;
            entity.Notes = dto.Notes;
            entity.Active = dto.Active;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Penalty>().UpdateOneAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Penalty>().FindByIdAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                entity.Active = false;
                entity.UpdatedAt = DateTime.UtcNow;
                
                await _unitOfWork.Repository<Penalty>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<bool> PenaltyExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Penalty>().AnyAsync(x => x.Id == id && !x.Deleted);
        }

        public async Task<IEnumerable<PenaltyResponseDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.Repository<Penalty>().FindManyAsync(x => !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<PenaltyResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Penalty>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return MapToResponseDTO(entity);
        }

        public async Task<IEnumerable<PenaltyResponseDTO>> GetByMemberAsync(string memberId)
        {
            var items = await _unitOfWork.Repository<Penalty>().FindManyAsync(x => x.MemberId == memberId && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<PenaltyResponseDTO>> GetByRentalAsync(int rentalId)
        {
            var items = await _unitOfWork.Repository<Penalty>().FindManyAsync(x => x.RentalId == rentalId && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<PenaltyResponseDTO>> GetByTypeAsync(string penaltyType)
        {
            var items = await _unitOfWork.Repository<Penalty>().FindManyAsync(x => x.PenaltyType == penaltyType && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<PenaltyResponseDTO>> GetPendingPenaltiesAsync()
        {
            var items = await _unitOfWork.Repository<Penalty>().FindManyAsync(x => x.PenaltyStatus == "Beklemede" && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<PenaltyResponseDTO>> GetApprovedPenaltiesAsync()
        {
            var items = await _unitOfWork.Repository<Penalty>().FindManyAsync(x => x.PenaltyStatus == "Onaylandı" && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<PenaltyResponseDTO>> GetPaidPenaltiesAsync()
        {
            var items = await _unitOfWork.Repository<Penalty>().FindManyAsync(x => x.PenaltyStatus == "Ödendi" && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task ApprovePenaltyAsync(int penaltyId, string approvedBy)
        {
            var entity = await _unitOfWork.Repository<Penalty>().FindByIdAsync(penaltyId);
            if (entity != null && !entity.Deleted)
            {
                entity.PenaltyStatus = "Onaylandı";
                entity.ApprovedBy = approvedBy;
                entity.ApprovedDate = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<Penalty>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task RejectPenaltyAsync(int penaltyId)
        {
            var entity = await _unitOfWork.Repository<Penalty>().FindByIdAsync(penaltyId);
            if (entity != null && !entity.Deleted)
            {
                entity.PenaltyStatus = "Kaldırıldı";
                entity.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<Penalty>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task MarkAsPaidAsync(int penaltyId)
        {
            var entity = await _unitOfWork.Repository<Penalty>().FindByIdAsync(penaltyId);
            if (entity != null && !entity.Deleted)
            {
                entity.PenaltyStatus = "Ödendi";
                entity.PaidDate = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<Penalty>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<decimal> GetTotalPenaltyByMemberAsync(string memberId)
        {
            var items = await _unitOfWork.Repository<Penalty>().FindManyAsync(x => x.MemberId == memberId && !x.Deleted);
            return items.Select(x => x.PenaltyAmount).Sum();
        }

        public async Task<decimal> GetTotalPenaltyByRentalAsync(int rentalId)
        {
            var items = await _unitOfWork.Repository<Penalty>().FindManyAsync(x => x.RentalId == rentalId && !x.Deleted);
            return items.Select(x => x.PenaltyAmount).Sum();
        }

        public async Task<IEnumerable<PenaltyResponseDTO>> GetPenaltiesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var items = await _unitOfWork.Repository<Penalty>().FindManyAsync(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        private static PenaltyResponseDTO MapToResponseDTO(Penalty entity)
        {
            return new PenaltyResponseDTO
            {
                Id = entity.Id,
                RentalId = entity.RentalId,
                MemberId = entity.MemberId,
                VehicleId = entity.VehicleId,
                PenaltyType = entity.PenaltyType,
                Description = entity.Description,
                PenaltyAmount = entity.PenaltyAmount,
                PenaltyStatus = entity.PenaltyStatus,
                ApprovedDate = entity.ApprovedDate,
                ApprovedBy = entity.ApprovedBy,
                PaidDate = entity.PaidDate,
                PhotosPath = entity.PhotosPath,
                Notes = entity.Notes,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Active = entity.Active
            };
        }
    }
}