using Business.Services;
using Core.Concretes.DTOs.Member;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.UI.Controllers
{
    [Authorize] // Depending on role logic, you might want [Authorize(Roles = "Admin")]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IRentalService _rentalService;
        private readonly IInvoiceService _invoiceService;
        private readonly IReviewService _reviewService;

        public MemberController(IMemberService memberService, IRentalService rentalService, IInvoiceService invoiceService, IReviewService reviewService)
        {
            _memberService = memberService;
            _rentalService = rentalService;
            _invoiceService = invoiceService;
            _reviewService = reviewService;
        }

        public async Task<IActionResult> Index()
        {
            var members = await _memberService.GetAllAsync();
            return View(members);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var member = await _memberService.GetByIdAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            ViewBag.Rentals = await _rentalService.GetByMemberAsync(id);
            ViewBag.Invoices = await _invoiceService.GetByMemberAsync(id);
            ViewBag.Reviews = await _reviewService.GetByMemberAsync(id);

            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyMember(string id)
        {
            var exists = await _memberService.MemberExistsAsync(id);
            if (!exists) return NotFound();

            await _memberService.VerifyMemberAsync(id);
            TempData["SuccessMessage"] = "Üye başarıyla doğrulandı.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlacklistMember(string id, string reason)
        {
            var exists = await _memberService.MemberExistsAsync(id);
            if (!exists) return NotFound();

            await _memberService.BlacklistMemberAsync(id, reason);
            TempData["SuccessMessage"] = "Üye kara listeye alındı.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnblacklistMember(string id)
        {
            var exists = await _memberService.MemberExistsAsync(id);
            if (!exists) return NotFound();

            await _memberService.UnblacklistMemberAsync(id);
            TempData["SuccessMessage"] = "Üye kara listeden çıkarıldı.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
