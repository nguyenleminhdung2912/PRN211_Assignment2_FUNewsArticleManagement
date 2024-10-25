using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObjects;
using DataAccessObjects;
using Repository.IRepository;
using Repository.Repository;
using Microsoft.AspNetCore.SignalR;

namespace NguyenLeMinhDungFall2024RazorPages.Pages.Admin
{
    public class AdminCreateUserModel : PageModel
    {
        private readonly ISystemAccountRepository systemAccountRepository;
        private readonly IHubContext<SignalRHub> hubContext;


        public AdminCreateUserModel(IHubContext<SignalRHub> hubContext)
        {
            systemAccountRepository = new SystemAccountRepository();
            this.hubContext = hubContext;
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

            await hubContext.Clients.All.SendAsync("RefreshData");

            return RedirectToPage("/Admin/ManageAccounts");
        }
    }
}
