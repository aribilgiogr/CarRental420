using Business.Services;
using Core.Concretes.DTOs.Campaign;
using Core.Concretes.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.UI.Controllers
{
    [Authorize]
    public class CampaignController : Controller
    {
        private readonly ICampaignService _campaignService;

        public CampaignController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        public async Task<IActionResult> Index()
        {
            var campaigns = await _campaignService.GetAllAsync();
            return View(campaigns);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CampaignTypes = new SelectList(Enum.GetValues(typeof(CampaignType)).Cast<CampaignType>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text");

            ViewBag.CampaignScopes = new SelectList(Enum.GetValues(typeof(CampaignScope)).Cast<CampaignScope>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text");

            return View(new CreateCampaignDTO { IsActive = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCampaignDTO dto)
        {
            if (ModelState.IsValid)
            {
                dto.CreatedBy = User.Identity?.Name ?? "Admin";
                await _campaignService.CreateAsync(dto);
                TempData["SuccessMessage"] = "Kampanya başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CampaignTypes = new SelectList(Enum.GetValues(typeof(CampaignType)).Cast<CampaignType>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text");

            ViewBag.CampaignScopes = new SelectList(Enum.GetValues(typeof(CampaignScope)).Cast<CampaignScope>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text");

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id)
        {
            await _campaignService.ActivateCampaignAsync(id);
            TempData["SuccessMessage"] = "Kampanya aktifleştirildi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(int id)
        {
            await _campaignService.DeactivateCampaignAsync(id);
            TempData["SuccessMessage"] = "Kampanya deaktif edildi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _campaignService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Kampanya silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
