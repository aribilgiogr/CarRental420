using Business.Services;
using Core.Concretes.DTOs.Penalty;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.UI.Controllers
{
    [Authorize]
    public class PenaltyController : Controller
    {
        private readonly IPenaltyService _penaltyService;
        private readonly IRentalService _rentalService;
        private readonly IMemberService _memberService;
        private readonly IVehicleService _vehicleService;

        public PenaltyController(IPenaltyService penaltyService, IRentalService rentalService, IMemberService memberService, IVehicleService vehicleService)
        {
            _penaltyService = penaltyService;
            _rentalService = rentalService;
            _memberService = memberService;
            _vehicleService = vehicleService;
        }

        public async Task<IActionResult> Index()
        {
            var penalties = await _penaltyService.GetAllAsync();
            return View(penalties);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int? rentalId)
        {
            if (rentalId.HasValue)
            {
                var rental = await _rentalService.GetByIdAsync(rentalId.Value);
                if (rental != null)
                {
                    ViewBag.Rentals = new SelectList(new[] { rental }, "Id", "Id", rental.Id);
                    ViewBag.Members = new SelectList(await _memberService.GetAllAsync(), "Id", "Email", rental.MemberId);
                    ViewBag.Vehicles = new SelectList(await _vehicleService.GetAllAsync(), "Id", "LicensePlate", rental.VehicleId);
                    return View(new CreatePenaltyDTO { RentalId = rental.Id, MemberId = rental.MemberId, VehicleId = rental.VehicleId });
                }
            }

            ViewBag.Rentals = new SelectList(await _rentalService.GetAllAsync(), "Id", "Id");
            ViewBag.Members = new SelectList(await _memberService.GetAllAsync(), "Id", "Email");
            ViewBag.Vehicles = new SelectList(await _vehicleService.GetAllAsync(), "Id", "LicensePlate");
            return View(new CreatePenaltyDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePenaltyDTO dto)
        {
            if (ModelState.IsValid)
            {
                dto.PenaltyStatus = "Beklemede";
                await _penaltyService.CreateAsync(dto);
                TempData["SuccessMessage"] = "Masraf/Ceza başarıyla kaydedildi.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Rentals = new SelectList(await _rentalService.GetAllAsync(), "Id", "Id");
            ViewBag.Members = new SelectList(await _memberService.GetAllAsync(), "Id", "Email");
            ViewBag.Vehicles = new SelectList(await _vehicleService.GetAllAsync(), "Id", "LicensePlate");
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            await _penaltyService.MarkAsPaidAsync(id);
            TempData["SuccessMessage"] = "Masraf ödendi olarak işaretlendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _penaltyService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Masraf/Ceza silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
