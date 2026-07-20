using Business.Services;
using Core.Concretes.DTOs.MaintenanceRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.UI.Controllers
{
    [Authorize]
    public class MaintenanceRecordController : Controller
    {
        private readonly IMaintenanceRecordService _maintenanceRecordService;
        private readonly IVehicleService _vehicleService;

        public MaintenanceRecordController(IMaintenanceRecordService maintenanceRecordService, IVehicleService vehicleService)
        {
            _maintenanceRecordService = maintenanceRecordService;
            _vehicleService = vehicleService;
        }

        public async Task<IActionResult> Index()
        {
            var records = await _maintenanceRecordService.GetAllAsync();
            // We could join vehicles here for display if we created a custom ViewModel, but we'll keep it simple for now.
            return View(records);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? vehicleId)
        {
            await PopulateVehiclesAsync();
            var model = new CreateMaintenanceRecordDTO();
            if (vehicleId.HasValue)
            {
                model.VehicleId = vehicleId.Value;
            }
            model.MaintenanceDate = DateTime.Today;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMaintenanceRecordDTO model)
        {
            if (ModelState.IsValid)
            {
                await _maintenanceRecordService.CreateAsync(model);
                TempData["SuccessMessage"] = "Bakım kaydı başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            await PopulateVehiclesAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var record = await _maintenanceRecordService.GetByIdAsync(id);
            if (record == null)
            {
                return NotFound();
            }

            var model = new UpdateMaintenanceRecordDTO
            {
                Id = record.Id,
                VehicleId = record.VehicleId,
                MaintenanceDate = record.MaintenanceDate,
                MaintenanceType = record.MaintenanceType,
                Description = record.Description,
                Cost = record.Cost,
                ServiceProvider = record.ServiceProvider,
                MaintenanceStatus = record.MaintenanceStatus,
                CompletionDate = record.CompletionDate,
                Odometer = record.Odometer,
                Notes = record.Notes,
                Attachments = record.Attachments,
                Active = record.Active
            };

            await PopulateVehiclesAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateMaintenanceRecordDTO model)
        {
            if (ModelState.IsValid)
            {
                await _maintenanceRecordService.UpdateAsync(model);
                TempData["SuccessMessage"] = "Bakım kaydı başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            await PopulateVehiclesAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _maintenanceRecordService.MaintenanceRecordExistsAsync(id);
            if (!exists)
            {
                return NotFound();
            }

            await _maintenanceRecordService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Bakım kaydı başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateVehiclesAsync()
        {
            var vehicles = await _vehicleService.GetAllAsync();
            var vehicleList = vehicles.Select(v => new { Id = v.Id, DisplayText = $"{v.LicensePlate} ({v.Brand} {v.Model})" });
            ViewBag.Vehicles = new SelectList(vehicleList, "Id", "DisplayText");
        }
    }
}
