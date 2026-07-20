using Business.Services;
using Core.Concretes.DTOs.Location;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.UI.Controllers
{
    [Authorize]
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public async Task<IActionResult> Index()
        {
            var locations = await _locationService.GetAllAsync();
            return View(locations);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateLocationDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLocationDTO model)
        {
            if (ModelState.IsValid)
            {
                await _locationService.CreateAsync(model);
                // Optionally add a success message using TempData
                TempData["SuccessMessage"] = "Şube başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var location = await _locationService.GetByIdAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            var model = new UpdateLocationDTO
            {
                Id = location.Id,
                Name = location.Name,
                City = location.City,
                District = location.District,
                Street = location.Street,
                PostalCode = location.PostalCode,
                PhoneNumber = location.PhoneNumber,
                Email = location.Email,
                Manager = location.Manager,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                OpeningTime = location.OpeningTime,
                ClosingTime = location.ClosingTime,
                IsOpen24Hours = location.IsOpen24Hours,
                Notes = location.Notes,
                Active = location.Active
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateLocationDTO model)
        {
            if (ModelState.IsValid)
            {
                await _locationService.UpdateAsync(model);
                TempData["SuccessMessage"] = "Şube başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _locationService.LocationExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            await _locationService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Şube başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
