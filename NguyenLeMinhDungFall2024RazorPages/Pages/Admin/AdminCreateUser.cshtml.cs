using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects;
using DataAccessObjects;
using Repository.IRepository;
using Repository.Repository;

namespace NguyenLeMinhDungFall2024RazorPages.Pages.Admin
{
    public class AdminCreateUserModel : PageModel
    {
        private readonly ISystemAccountRepository systemAccountRepository;

        public AdminCreateUserModel()
        {
            systemAccountRepository = new SystemAccountRepository();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = default!;
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            systemAccountRepository.SaveAccount(SystemAccount);

            return RedirectToPage("/Admin/ManageAccounts");
        }
    }
}
