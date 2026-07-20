using Business.Services;
using Core.Concretes.DTOs.Insurance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.UI.Controllers
{
    [Authorize]
    public class InsuranceController : Controller
    {
        private readonly IInsuranceService _insuranceService;

        public InsuranceController(IInsuranceService insuranceService)
        {
            _insuranceService = insuranceService;
        }

        public async Task<IActionResult> Index()
        {
            var insurances = await _insuranceService.GetAllAsync();
            return View(insurances);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInsuranceDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _insuranceService.CreateAsync(dto);
                TempData["SuccessMessage"] = "Yeni sigorta başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _insuranceService.InsuranceExistsAsync(id);
            if (!exists) return NotFound();

            await _insuranceService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Sigorta poliçesi başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
