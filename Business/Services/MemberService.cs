using Core.Concretes.DTOs.Member;
using Core.Concretes.Entities;
using Core.Utils;

namespace Business.Services
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task UpdateAsync(UpdateMemberDTO dto)
        {
            var entity = await _unitOfWork.Repository<Member>().FindByIdAsync(dto.Id);
            if (entity == null || entity.Deleted) return;

            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.PhoneNumber2 = dto.PhoneNumber2;
            entity.BirthDate = dto.BirthDate;
            entity.DriverLicenseNumber = dto.DriverLicenseNumber;
            entity.DriverLicenseExpiryDate = dto.DriverLicenseExpiryDate;
            entity.NationalIdNumber = dto.NationalIdNumber;
            entity.ProfileImageUrl = dto.ProfileImageUrl;
            entity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Member>().UpdateOneAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await _unitOfWork.Repository<Member>().FindByIdAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                entity.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<Member>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task<bool> MemberExistsAsync(string id)
        {
            return await _unitOfWork.Repository<Member>().AnyAsync(x => x.Id == id && !x.Deleted);
        }

        public async Task<IEnumerable<MemberResponseDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.Repository<Member>().FindManyAsync(x => !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<MemberResponseDTO?> GetByIdAsync(string id)
        {
            var entity = await _unitOfWork.Repository<Member>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return MapToResponseDTO(entity);
        }

        public async Task<IEnumerable<MemberResponseDTO>> GetBlacklistedMembersAsync()
        {
            var items = await _unitOfWork.Repository<Member>().FindManyAsync(x => x.IsBlacklisted && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task<IEnumerable<MemberResponseDTO>> GetVerifiedMembersAsync()
        {
            var items = await _unitOfWork.Repository<Member>().FindManyAsync(x => x.IsVerified && !x.Deleted);
            return items.Select(MapToResponseDTO);
        }

        public async Task VerifyMemberAsync(string memberId)
        {
            var entity = await _unitOfWork.Repository<Member>().FindByIdAsync(memberId);
            if (entity != null && !entity.Deleted)
            {
                entity.IsVerified = true;
                entity.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Repository<Member>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task BlacklistMemberAsync(string memberId, string reason)
        {
            var entity = await _unitOfWork.Repository<Member>().FindByIdAsync(memberId);
            if (entity != null && !entity.Deleted)
            {
                entity.IsBlacklisted = true;
                entity.BlacklistReason = reason;
                entity.BlacklistDate = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<Member>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task UnblacklistMemberAsync(string memberId)
        {
            var entity = await _unitOfWork.Repository<Member>().FindByIdAsync(memberId);
            if (entity != null && !entity.Deleted)
            {
                entity.IsBlacklisted = false;
                entity.BlacklistReason = null;
                entity.BlacklistDate = null;
                entity.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<Member>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        private static MemberResponseDTO MapToResponseDTO(Member entity)
        {
            return new MemberResponseDTO
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                PhoneNumber2 = entity.PhoneNumber2,
                BirthDate = entity.BirthDate,
                DriverLicenseNumber = entity.DriverLicenseNumber,
                DriverLicenseExpiryDate = entity.DriverLicenseExpiryDate,
                NationalIdNumber = entity.NationalIdNumber,
                ProfileImageUrl = entity.ProfileImageUrl,
                IsVerified = entity.IsVerified,
                IsBlacklisted = entity.IsBlacklisted,
                BlacklistReason = entity.BlacklistReason,
                BlacklistDate = entity.BlacklistDate,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Deleted = entity.Deleted
            };
        }
    }
}