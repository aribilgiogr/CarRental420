using AutoMapper;
using Core.Concretes.DTOs.Campaign;
using Core.Concretes.Entities;
using Core.Concretes.Enums;
using Core.Utils;

namespace Business.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CampaignService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Yeni bir kampanya oluşturur ve veritabanına kaydeder.
        /// </summary>
        /// <param name="dto">Kampanya oluşturmak için gerekli bilgiler</param>
        /// <returns>Oluşturulan kampanyanın DTO şeklindeki gösterimi</returns>
        public async Task<CampaignResponseDTO> CreateAsync(CreateCampaignDTO dto)
        {
            var entity = _mapper.Map<Campaign>(dto);
            await _unitOfWork.Repository<Campaign>().InsertOneAsync(entity);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<CampaignResponseDTO>(entity);
        }

        /// <summary>
        /// Mevcut bir kampanyayı günceller. Silinmiş kampanyalar güncellenemez.
        /// </summary>
        /// <param name="dto">Güncellenecek kampanyanın yeni bilgileri</param>
        public async Task UpdateAsync(UpdateCampaignDTO dto)
        {
            var entity = await _unitOfWork.Repository<Campaign>().FindByIdAsync(dto.Id);
            if (entity == null || entity.Deleted) return;

            entity = _mapper.Map<Campaign>(dto);

            await _unitOfWork.Repository<Campaign>().UpdateOneAsync(entity);
            await _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Bir kampanyayı soft delete yöntemiyle siler. Deleted bayrağını true olarak işaretler.
        /// </summary>
        /// <param name="id">Silinecek kampanyanın ID'si</param>
        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Campaign>().FindByIdAsync(id);
            if (entity != null)
            {
                entity.Deleted = true;
                entity.Active = false;
                entity.IsActive = false;
                entity.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<Campaign>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        /// <summary>
        /// Belirtilen ID'ye sahip kampanyayı getirir. Silinmiş kampanyalar döndürülmez.
        /// </summary>
        /// <param name="id">Kampanyanın ID'si</param>
        /// <returns>Bulunursa kampanya DTO'su, bulunamazsa null</returns>
        public async Task<CampaignResponseDTO?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Campaign>().FindByIdAsync(id);
            if (entity == null || entity.Deleted) return null;

            return _mapper.Map<CampaignResponseDTO>(entity);
        }

        /// <summary>
        /// Silinmemiş tüm kampanyaları getirir.
        /// </summary>
        /// <returns>Kampanya DTO'larının koleksiyonu</returns>
        public async Task<IEnumerable<CampaignResponseDTO>> GetAllAsync()
        {
            var items = await _unitOfWork.Repository<Campaign>().FindManyAsync(x => !x.Deleted);
            return _mapper.Map<IEnumerable<CampaignResponseDTO>>(items);
        }

        /// <summary>
        /// Aktif ve silinmemiş kampanyaları getirir.
        /// </summary>
        /// <returns>Aktif kampanya DTO'larının koleksiyonu</returns>
        public async Task<IEnumerable<CampaignResponseDTO>> GetActiveCampaignsAsync()
        {
            var items = await _unitOfWork.Repository<Campaign>().FindManyAsync(x => !x.Deleted && x.IsActive);
            return _mapper.Map<IEnumerable<CampaignResponseDTO>>(items);
        }

        /// <summary>
        /// Belirtilen türdeki kampanyaları getirir (örn: TimeLimited, Unlimited).
        /// </summary>
        /// <param name="campaignType">Filtrelenecek kampanya türü</param>
        /// <returns>Belirtilen türdeki kampanya DTO'larının koleksiyonu</returns>
        public async Task<IEnumerable<CampaignResponseDTO>> GetByTypeAsync(CampaignType campaignType)
        {
            var items = await _unitOfWork.Repository<Campaign>().FindManyAsync(x => !x.Deleted && x.CampaignType == campaignType);
            return _mapper.Map<IEnumerable<CampaignResponseDTO>>(items);
        }

        /// <summary>
        /// Belirtilen kapsamdaki kampanyaları getirir (örn: Local, Global).
        /// </summary>
        /// <param name="campaignScope">Filtrelenecek kampanya kapsamı</param>
        /// <returns>Belirtilen kapsamdaki kampanya DTO'larının koleksiyonu</returns>
        public async Task<IEnumerable<CampaignResponseDTO>> GetByScopeAsync(CampaignScope campaignScope)
        {
            var items = await _unitOfWork.Repository<Campaign>().FindManyAsync(x => !x.Deleted && x.CampaignScope == campaignScope);
            return _mapper.Map<IEnumerable<CampaignResponseDTO>>(items);
        }

        /// <summary>
        /// Belirtilen kupon koduna sahip kampanyayı getirir.
        /// </summary>
        /// <param name="couponCode">Aranacak kupon kodu</param>
        /// <returns>Bulunursa kampanya DTO'su, bulunamazsa null</returns>
        public async Task<CampaignResponseDTO?> GetByCouponCodeAsync(string couponCode)
        {
            var items = await _unitOfWork.Repository<Campaign>().FindManyAsync(x => !x.Deleted && x.CouponCode == couponCode);
            var entity = items.FirstOrDefault();

            return entity != null ? _mapper.Map<CampaignResponseDTO>(entity) : null;
        }

        /// <summary>
        /// Zaman sınırlı kampanyaları getirir.
        /// </summary>
        /// <returns>Zaman sınırlı kampanya DTO'larının koleksiyonu</returns>
        public async Task<IEnumerable<CampaignResponseDTO>> GetTimeBasedCampaignsAsync()
        {
            return await GetByTypeAsync(CampaignType.TimeLimited);
        }

        /// <summary>
        /// Sınırsız kampanyaları getirir (zaman sınırı olmayan).
        /// </summary>
        /// <returns>Sınırsız kampanya DTO'larının koleksiyonu</returns>
        public async Task<IEnumerable<CampaignResponseDTO>> GetUnlimitedCampaignsAsync()
        {
            return await GetByTypeAsync(CampaignType.Unlimited);
        }

        /// <summary>
        /// Belirtilen ID'ye sahip silinmemiş bir kampanyanın var olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="id">Kontrol edilecek kampanyanın ID'si</param>
        /// <returns>Kampanya varsa true, yoksa false</returns>
        public async Task<bool> CampaignExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Campaign>().AnyAsync(x => x.Id == id && !x.Deleted);
        }

        /// <summary>
        /// Belirtilen kupon kodunun var olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="couponCode">Kontrol edilecek kupon kodu</param>
        /// <returns>Kupon kodu varsa true, yoksa false</returns>
        public async Task<bool> CouponCodeExistsAsync(string couponCode)
        {
            return await _unitOfWork.Repository<Campaign>().AnyAsync(x => x.CouponCode == couponCode && !x.Deleted);
        }

        /// <summary>
        /// Belirtilen kampanyayı aktif hale getirir.
        /// </summary>
        /// <param name="campaignId">Aktif hale getirilecek kampanyanın ID'si</param>
        public async Task ActivateCampaignAsync(int campaignId)
        {
            var entity = await _unitOfWork.Repository<Campaign>().FindByIdAsync(campaignId);
            if (entity != null && !entity.Deleted)
            {
                entity.IsActive = true;
                entity.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Repository<Campaign>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        /// <summary>
        /// Belirtilen kampanyayı deaktif hale getirir.
        /// </summary>
        /// <param name="campaignId">Deaktif hale getirilecek kampanyanın ID'si</param>
        public async Task DeactivateCampaignAsync(int campaignId)
        {
            var entity = await _unitOfWork.Repository<Campaign>().FindByIdAsync(campaignId);
            if (entity != null && !entity.Deleted)
            {
                entity.IsActive = false;
                entity.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Repository<Campaign>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        /// <summary>
        /// Belirtilen kampanya için kiralama fiyatına göre indirim miktarını hesaplar.
        /// Sabit tutar ve yüzde indirimlerinin toplamını döndürür, fakat kiralama fiyatını aşamaz.
        /// </summary>
        /// <param name="campaignId">İndirim hesaplanacak kampanyanın ID'si</param>
        /// <param name="rentalPrice">Kiralama fiyatı</param>
        /// <returns>Hesaplanan indirim miktarı</returns>
        public async Task<decimal> CalculateDiscountAsync(int campaignId, decimal rentalPrice)
        {
            var campaign = await _unitOfWork.Repository<Campaign>().FindByIdAsync(campaignId);
            if (campaign == null || campaign.Deleted || !campaign.IsActive) return 0;

            decimal discount = 0;

            if (campaign.DiscountFixedAmount.HasValue && campaign.DiscountFixedAmount > 0)
            {
                discount += campaign.DiscountFixedAmount.Value;
            }

            if (campaign.DiscountPercentage > 0)
            {
                discount += (rentalPrice * campaign.DiscountPercentage) / 100;
            }

            return discount > rentalPrice ? rentalPrice : discount;
        }

        /// <summary>
        /// Belirtilen kampanyanın bir üye tarafından uygulanabilir olup olmadığını kontrol eder.
        /// Kampanyanın aktif olması, minimum tutar, maksimum kullanım sayısı ve zaman aralığı kontrol edilir.
        /// </summary>
        /// <param name="campaignId">Kontrol edilecek kampanyanın ID'si</param>
        /// <param name="memberId">Üyenin ID'si</param>
        /// <param name="rentalAmount">Kiralama tutarı</param>
        /// <returns>Kampanya uygulanabilirse true, yoksa false</returns>
        public async Task<bool> CanApplyCampaignAsync(int campaignId, string memberId, decimal rentalAmount)
        {
            var campaign = await _unitOfWork.Repository<Campaign>().FindByIdAsync(campaignId);

            if (campaign == null || campaign.Deleted || !campaign.IsActive)
                return false;

            if (campaign.MinimumRentalAmount.HasValue && rentalAmount < campaign.MinimumRentalAmount.Value)
                return false;

            if (campaign.MaxUsageCount.HasValue && campaign.CurrentUsageCount >= campaign.MaxUsageCount.Value)
                return false;

            if (campaign.CampaignType == CampaignType.TimeLimited)
            {
                var now = DateTime.UtcNow;
                if ((campaign.StartDate.HasValue && now < campaign.StartDate.Value) ||
                    (campaign.EndDate.HasValue && now > campaign.EndDate.Value))
                {
                    return false;
                }
            }

            // Üye bazlı kullanım sınırı için RentalCampaign geçmişi kontrolü gerekir.
            // Sadece bu entity tabanlı kısıtlamalar yukarıda yapılmıştır.
            return true;
        }

        /// <summary>
        /// Belirtilen kampanyanın kullanım sayısını bir artırır.
        /// </summary>
        /// <param name="campaignId">Kullanım sayısı artırılacak kampanyanın ID'si</param>
        public async Task IncrementUsageCountAsync(int campaignId)
        {
            var entity = await _unitOfWork.Repository<Campaign>().FindByIdAsync(campaignId);
            if (entity != null && !entity.Deleted)
            {
                entity.CurrentUsageCount = (entity.CurrentUsageCount ?? 0) + 1;
                entity.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<Campaign>().UpdateOneAsync(entity);
                await _unitOfWork.CommitAsync();
            }
        }

        /// <summary>
        /// Belirtilen kiralama tutarı için uygulanabilir kampanyaları getirir.
        /// Minimum tutar, maksimum kullanım sayısı ve zaman aralığı koşullarını kontrol eder.
        /// </summary>
        /// <param name="rentalAmount">Kiralama tutarı</param>
        /// <returns>Uygulanabilir kampanya DTO'larının koleksiyonu</returns>
        public async Task<IEnumerable<CampaignResponseDTO>> GetApplicableCampaignsAsync(decimal rentalAmount)
        {
            var now = DateTime.UtcNow;

            var items = await _unitOfWork.Repository<Campaign>().FindManyAsync(x =>
                !x.Deleted &&
                x.IsActive &&
                (!x.MinimumRentalAmount.HasValue || x.MinimumRentalAmount <= rentalAmount) &&
                (!x.MaxUsageCount.HasValue || x.CurrentUsageCount < x.MaxUsageCount) &&
                (x.CampaignType != CampaignType.TimeLimited || ((!x.StartDate.HasValue || x.StartDate <= now) && (!x.EndDate.HasValue || x.EndDate >= now)))
            );

            return _mapper.Map<IEnumerable<CampaignResponseDTO>>(items);
        }
    }
}