using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NguyenLeMinhDungFall2024RazorPages.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminIndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
