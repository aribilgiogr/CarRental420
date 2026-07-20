using Business.Services;
using Core.Concretes.DTOs.Rental;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.UI.Controllers
{
    [Authorize]
    public class RentalController : Controller
    {
        private readonly IRentalService _rentalService;
        private readonly IMemberService _memberService;
        private readonly IVehicleService _vehicleService;
        private readonly ILocationService _locationService;

        public RentalController(
            IRentalService rentalService,
            IMemberService memberService,
            IVehicleService vehicleService,
            ILocationService locationService)
        {
            _rentalService = rentalService;
            _memberService = memberService;
            _vehicleService = vehicleService;
            _locationService = locationService;
        }

        public async Task<IActionResult> Index()
        {
            var rentals = await _rentalService.GetAllAsync();
            return View(rentals);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var rental = await _rentalService.GetByIdAsync(id);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownsAsync();
            var model = new CreateRentalDTO
            {
                RentalStartDate = DateTime.Today,
                RentalEndDate = DateTime.Today.AddDays(1),
                RentalStatus = "Beklemede"
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRentalDTO dto)
        {
            if (ModelState.IsValid)
            {
                // Basit kiralama işlemi, araç kilometre kontrolü falan servise veya UI tarafına bırakılabilir.
                await _rentalService.CreateAsync(dto);
                TempData["SuccessMessage"] = "Kiralama başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownsAsync();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete(int id, int endOdometer)
        {
            var exists = await _rentalService.RentalExistsAsync(id);
            if (!exists) return NotFound();

            await _rentalService.CompleteRentalAsync(id, endOdometer);
            TempData["SuccessMessage"] = "Kiralama başarıyla tamamlandı (Araç teslim alındı).";
            return RedirectToAction(nameof(Details), new { id });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _rentalService.RentalExistsAsync(id);
            if (!exists) return NotFound();

            await _rentalService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Kiralama başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdownsAsync()
        {
            var members = await _memberService.GetAllAsync();
            var memberList = members.Select(m => new { m.Id, FullName = $"{m.FirstName} {m.LastName} ({m.NationalIdNumber})" });
            
            var vehicles = await _vehicleService.GetAllAsync();
            var vehicleList = vehicles.Where(v => v.IsAvailable && v.Active)
                                      .Select(v => new { v.Id, DisplayName = $"{v.LicensePlate} - {v.Brand} {v.Model}" });
                                      
            var locations = await _locationService.GetAllAsync();

            ViewBag.Members = new SelectList(memberList, "Id", "FullName");
            ViewBag.Vehicles = new SelectList(vehicleList, "Id", "DisplayName");
            ViewBag.Locations = new SelectList(locations, "Id", "Name");
        }
    }
}
