using Business.Services;
using Core.Concretes.DTOs.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.UI.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<IActionResult> Index()
        {
            var payments = await _paymentService.GetAllAsync();
            return View(payments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePaymentDTO dto)
        {
            if (dto.PaymentDate == default) dto.PaymentDate = DateTime.Now;
            if (string.IsNullOrEmpty(dto.TransactionNumber))
            {
                dto.TransactionNumber = "TRX-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
            }

            if (ModelState.IsValid)
            {
                await _paymentService.CreateAsync(dto);
                TempData["SuccessMessage"] = "Ödeme başarıyla sisteme kaydedildi.";
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsFailed(int id)
        {
            var exists = await _paymentService.PaymentExistsAsync(id);
            if (!exists) return NotFound();

            await _paymentService.MarkAsFailedAsync(id);
            TempData["SuccessMessage"] = "Ödeme işlemi başarısız/iptal olarak işaretlendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _paymentService.PaymentExistsAsync(id);
            if (!exists) return NotFound();

            await _paymentService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Ödeme kaydı silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
