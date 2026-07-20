using Business.Services;
using Core.Concretes.DTOs.Invoice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.UI.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        public async Task<IActionResult> Index()
        {
            var invoices = await _invoiceService.GetAllAsync();
            return View(invoices);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInvoiceDTO dto)
        {
            if (string.IsNullOrEmpty(dto.InvoiceNumber))
            {
                dto.InvoiceNumber = "INV-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
            }
            if (dto.InvoiceDate == default) dto.InvoiceDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                await _invoiceService.CreateAsync(dto);
                TempData["SuccessMessage"] = "Fatura başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var exists = await _invoiceService.InvoiceExistsAsync(id);
            if (!exists) return NotFound();

            await _invoiceService.MarkAsPaidAsync(id);
            TempData["SuccessMessage"] = "Fatura ödendi olarak işaretlendi.";
            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _invoiceService.InvoiceExistsAsync(id);
            if (!exists) return NotFound();

            await _invoiceService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Fatura kaydı silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
