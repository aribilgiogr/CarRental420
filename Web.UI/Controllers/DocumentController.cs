using Business.Services;
using Core.Concretes.DTOs.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.UI.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        public async Task<IActionResult> Index()
        {
            var documents = await _documentService.GetAllAsync();
            return View(documents);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Usually we'd want to load Members here for a dropdown.
            // For now, we will leave it as an input, or we can load members later.
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDocumentDTO dto)
        {
            dto.UploadDate = DateTime.UtcNow;
            if (ModelState.IsValid)
            {
                await _documentService.CreateAsync(dto);
                TempData["SuccessMessage"] = "Yeni evrak sisteme başarıyla yüklendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveDocument(int id)
        {
            var exists = await _documentService.DocumentExistsAsync(id);
            if (!exists) return NotFound();

            var currentUser = User.Identity?.Name ?? "Admin";
            await _documentService.ApproveDocumentAsync(id, currentUser);
            
            TempData["SuccessMessage"] = "Evrak başarıyla onaylandı.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectDocument(int id, string reason)
        {
            var exists = await _documentService.DocumentExistsAsync(id);
            if (!exists) return NotFound();

            await _documentService.RejectDocumentAsync(id, reason);
            TempData["SuccessMessage"] = "Evrak reddedildi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await _documentService.DocumentExistsAsync(id);
            if (!exists) return NotFound();

            await _documentService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Evrak başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
