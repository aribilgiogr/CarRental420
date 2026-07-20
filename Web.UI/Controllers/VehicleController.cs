using Business.Services;
using Core.Concretes.DTOs.Vehicle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.UI.Controllers
{
    [Authorize]
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IVehicleCategoryService _vehicleCategoryService;
        private readonly ILocationService _locationService;

        public VehicleController(
            IVehicleService vehicleService, 
            IVehicleCategoryService vehicleCategoryService, 
            ILocationService locationService)
        {
            _vehicleService = vehicleService;
            _vehicleCategoryService = vehicleCategoryService;
            _locationService = locationService;
        }

        public async Task<IActionResult> Index()
        {
            var vehicles = await _vehicleService.GetAllAsync();
            return View(vehicles);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            return View(new CreateVehicleDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVehicleDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _vehicleService.CreateAsync(dto);
                TempData["SuccessMessage"] = "Araç başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropdownsAsync();
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            var model = new UpdateVehicleDTO
            {
                Id = vehicle.Id,
                LicensePlate = vehicle.LicensePlate,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Year = vehicle.Year,
                Kilometers = vehicle.Kilometers,
                PricePerDay = vehicle.PricePerDay,
                VehicleType = vehicle.VehicleType,
                VehicleCategoryId = vehicle.VehicleCategoryId,
                VIN = vehicle.VIN,
                EngineNumber = vehicle.EngineNumber,
                Color = vehicle.Color,
                Seats = vehicle.Seats,
                FuelTankCapacity = vehicle.FuelTankCapacity,
                FuelType = vehicle.FuelType,
                TransmissionType = vehicle.TransmissionType,
                HasInsurance = vehicle.HasInsurance,
                InsuranceExpiryDate = vehicle.InsuranceExpiryDate,
                IsAvailable = vehicle.IsAvailable,
                RequiresInspection = vehicle.RequiresInspection,
                Notes = vehicle.Notes,
                LocationId = vehicle.LocationId,
                Active = vehicle.Active
            };

            await PopulateDropdownsAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateVehicleDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _vehicleService.UpdateAsync(dto);
                TempData["SuccessMessage"] = "Araç başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            await PopulateDropdownsAsync();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _vehicleService.VehicleExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            await _vehicleService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Araç başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }

            // You can load maintenance records and images here if needed
            return View(vehicle);
        }

        private async Task PopulateDropdownsAsync()
        {
            var categories = await _vehicleCategoryService.GetAllAsync();
            var locations = await _locationService.GetAllAsync();

            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.Locations = new SelectList(locations, "Id", "Name");
        }
    }
}
