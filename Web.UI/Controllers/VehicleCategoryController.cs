using Business.Services;
using Core.Concretes.DTOs.VehicleCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.UI.Controllers
{
    [Authorize]
    public class VehicleCategoryController : Controller
    {
        private readonly IVehicleCategoryService _vehicleCategoryService;

        public VehicleCategoryController(IVehicleCategoryService vehicleCategoryService)
        {
            _vehicleCategoryService = vehicleCategoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _vehicleCategoryService.GetAllAsync();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateVehicleCategoryDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVehicleCategoryDTO model)
        {
            if (ModelState.IsValid)
            {
                await _vehicleCategoryService.CreateAsync(model);
                TempData["SuccessMessage"] = "Kategori başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _vehicleCategoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var model = new UpdateVehicleCategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                DailyPrice = category.DailyPrice,
                WeeklyPrice = category.WeeklyPrice,
                MonthlyPrice = category.MonthlyPrice,
                MaxPassengers = category.MaxPassengers,
                BaggageCapacity = category.BaggageCapacity,
                HasAirConditioning = category.HasAirConditioning,
                HasAutomaticTransmission = category.HasAutomaticTransmission,
                HasGPS = category.HasGPS,
                ImageUrl = category.ImageUrl,
                Active = category.Active
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateVehicleCategoryDTO model)
        {
            if (ModelState.IsValid)
            {
                await _vehicleCategoryService.UpdateAsync(model);
                TempData["SuccessMessage"] = "Kategori başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _vehicleCategoryService.CategoryExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            await _vehicleCategoryService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Kategori başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
