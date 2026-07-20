using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.UI.Controllers
{
    [Authorize]
    public class BlacklistController : Controller
    {
        private readonly IMemberService _memberService;

        public BlacklistController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public async Task<IActionResult> Index()
        {
            var blacklistedMembers = await _memberService.GetBlacklistedMembersAsync();
            return View(blacklistedMembers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unblacklist(string id)
        {
            var member = await _memberService.GetByIdAsync(id);
            if (member == null) return NotFound();

            await _memberService.UnblacklistMemberAsync(id);
            TempData["SuccessMessage"] = $"{member.FirstName} {member.LastName} başarıyla kara listeden çıkarıldı.";
            
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BlacklistMember(string id, string reason)
        {
            var member = await _memberService.GetByIdAsync(id);
            if (member == null) return NotFound();

            if (string.IsNullOrWhiteSpace(reason))
            {
                reason = "Yönetici tarafından belirtilmedi.";
            }

            await _memberService.BlacklistMemberAsync(id, reason);
            TempData["SuccessMessage"] = $"{member.FirstName} {member.LastName} kara listeye eklendi.";
            
            return RedirectToAction("Index", "Member");
        }
    }
}
