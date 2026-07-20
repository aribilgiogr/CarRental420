using Business.Services;
using Core.Concretes.DTOs.Review;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.UI.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public async Task<IActionResult> Index()
        {
            var reviews = await _reviewService.GetAllAsync();
            return View(reviews);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReviewDTO dto)
        {
            if (dto.ReviewDate == default) dto.ReviewDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                await _reviewService.CreateAsync(dto);
                TempData["SuccessMessage"] = "Değerlendirme başarıyla sisteme eklendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublishReview(int id)
        {
            var exists = await _reviewService.ReviewExistsAsync(id);
            if (!exists) return NotFound();

            await _reviewService.PublishReviewAsync(id);
            TempData["SuccessMessage"] = "Değerlendirme yayınlandı.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnpublishReview(int id)
        {
            var exists = await _reviewService.ReviewExistsAsync(id);
            if (!exists) return NotFound();

            await _reviewService.UnpublishReviewAsync(id);
            TempData["SuccessMessage"] = "Değerlendirme yayından kaldırıldı.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _reviewService.ReviewExistsAsync(id);
            if (!exists) return NotFound();

            await _reviewService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Değerlendirme silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
