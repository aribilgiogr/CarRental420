using Core.Concretes.DTOs.Rental;
using Core.Concretes.Entities;
using Core.Utils;

namespace Business.Services
{
    public class RentalService : IRentalService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RentalService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RentalResponseDTO> CreateAsync(CreateRentalDTO dto)
        {
            var entity = new Rental
            {
                MemberId = dto.MemberId,
                VehicleId = dto.VehicleId,
                StartLocationId = dto.StartLocationId,
                EndLocationId = dto.EndLocationId,
                RentalStartDate = dto.RentalStartDate,
                RentalEndDate = dto.RentalEndDate,
                ActualReturnDate = dto.ActualReturnDate,
                StartOdometer = dto.StartOdometer,
                EndOdometer = dto.EndOdometer,
                TotalPrice = dto.TotalPrice,
                DiscountAmount = dto.DiscountAmount,
                FinalPrice = dto.FinalPrice,
                RentalStatus = dto.RentalStatus ?? "Aktif",
                IsReturned = dto.IsReturned,
                HasDamage = dto.HasDamage,
                DamageDescription = dto.DamageDescription,
                Notes = dto.Notes,
                RentalDays = dto.RentalDays,
                InsuranceId = dto.InsuranceId,
                Active = true,
                Deleted = false,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Rental>().InsertOneAsync(entity);
            await _unitOfWork.CommitAsync();

            return MapToResponseDTO(entity);
        }

        public async Task UpdateAsync(UpdateRentalDTO dto)
        {
            var entity = await _unitOfWork.Repository<Rental>().FindByIdAsync(dto.Id);
            if (entity == null || entity.Deleted) return;

            entity.MemberId = dto.MemberId;
            entity.VehicleId = dto.VehicleId;
            entity.StartLocationId = dto.StartLocationId;
            entity.EndLocationId = dto.EndLocationId;
            entity.RentalStartDate = dto.RentalStartDate;
            entity.RentalEndDate = dto.RentalEndDate;
            entity.ActualReturnDate = dto.ActualReturnDate;
            entity.StartOdometer = dto.StartOdometer;
            entity.EndOdometer = dto.EndOdometer;
            entity.TotalPrice = dto.TotalPrice;
            entity.DiscountAmount = dto.DiscountAmount;
            entity.FinalPrice = dto.FinalPrice;
            entity.RentalStatus = dto.RentalStatus;
            entity.IsReturned = dto.IsReturned;
            entity.HasDamage = dto.HasDamage;
            entity.DamageDescription = dto.DamageDescription;
            entity.Notes = dto.Notes;
            entity.RentalDays = dto.RentalDays;
            entity.InsuranceId = dto.InsuranceId;
            entity.Active = dto.Active;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Rental>().UpdateOneAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Rental>().FindByIdAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                entity.Active = false;
                entity.UpdatedAt = DateTime.UtcNow;
                
                await _unitOfWork.Repository<Rental>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<bool> RentalExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Rental>().AnyAsync(x => x.Id == id && !x.Deleted);
        }

        public async Task<IEnumerable<RentalResponseDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.Repository<Rental>().FindManyAsync(x => !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<RentalResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Rental>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return MapToResponseDTO(entity);
        }

        public async Task<IEnumerable<RentalResponseDTO>> GetByMemberAsync(string memberId)
        {
            var items = await _unitOfWork.Repository<Rental>().FindManyAsync(x => x.MemberId == memberId && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<RentalResponseDTO>> GetByVehicleAsync(int vehicleId)
        {
            var items = await _unitOfWork.Repository<Rental>().FindManyAsync(x => x.VehicleId == vehicleId && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<RentalResponseDTO>> GetActiveRentalsAsync()
        {
            var items = await _unitOfWork.Repository<Rental>().FindManyAsync(x => !x.IsReturned && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<RentalResponseDTO>> GetCompletedRentalsAsync()
        {
            var items = await _unitOfWork.Repository<Rental>().FindManyAsync(x => x.IsReturned && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<RentalResponseDTO>> GetOverdueRentalsAsync()
        {
            var now = DateTime.UtcNow;
            var items = await _unitOfWork.Repository<Rental>().FindManyAsync(x => !x.IsReturned && x.RentalEndDate < now && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task CompleteRentalAsync(int rentalId, int endOdometer)
        {
            var entity = await _unitOfWork.Repository<Rental>().FindByIdAsync(rentalId);
            if (entity != null && !entity.Deleted)
            {
                entity.IsReturned = true;
                entity.EndOdometer = endOdometer;
                entity.ActualReturnDate = DateTime.UtcNow;
                entity.RentalStatus = "Tamamlandı";
                entity.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Repository<Rental>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public Task ReturnVehicleAsync(int rentalId, int endOdometer, bool hasDamage = false, string? damageDescription = null)
        {
            // Ceza kesimi (Penalty), hasar kaydı veya ödeme tetiklemeleri gibi iş kuralları tam bilinmediğinden varsayım yapılmamıştır.
            throw new NotImplementedException();
        }

        public Task<decimal> CalculateRentalPriceAsync(int vehicleId, DateTime startDate, DateTime endDate)
        {
            // Sigorta, indirim (Campaign), vergi ve günlük periyot hesaplama algoritmaları bilinmediğinden varsayım yapılmamıştır.
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RentalResponseDTO>> GetRentalsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var items = await _unitOfWork.Repository<Rental>().FindManyAsync(x => 
                x.RentalStartDate >= startDate && x.RentalStartDate <= endDate && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        private static RentalResponseDTO MapToResponseDTO(Rental entity)
        {
            return new RentalResponseDTO
            {
                Id = entity.Id,
                MemberId = entity.MemberId,
                VehicleId = entity.VehicleId,
                StartLocationId = entity.StartLocationId,
                EndLocationId = entity.EndLocationId,
                RentalStartDate = entity.RentalStartDate,
                RentalEndDate = entity.RentalEndDate,
                ActualReturnDate = entity.ActualReturnDate,
                StartOdometer = entity.StartOdometer,
                EndOdometer = entity.EndOdometer,
                TotalPrice = entity.TotalPrice,
                DiscountAmount = entity.DiscountAmount,
                FinalPrice = entity.FinalPrice,
                RentalStatus = entity.RentalStatus,
                IsReturned = entity.IsReturned,
                HasDamage = entity.HasDamage,
                DamageDescription = entity.DamageDescription,
                Notes = entity.Notes,
                RentalDays = entity.RentalDays,
                InsuranceId = entity.InsuranceId,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Active = entity.Active
            };
        }
    }
}