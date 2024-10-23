using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NguyenLeMinhDungFall2024RazorPages.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            await HttpContext.SignOutAsync();
            return RedirectToPage("/Index"); // Chuyển hướng đến trang chính sau khi đăng xuất
        }

        public async Task<IActionResult> OnPost()
        {
            await HttpContext.SignOutAsync();
            return RedirectToPage("/Index"); // Chuyển hướng đến trang chính sau khi đăng xuất
        }
    }
}
