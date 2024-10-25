using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;
using Repository.Repository;

namespace NguyenLeMinhDungFall2024RazorPages.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ManageAccountsModel : PageModel
    {
        private readonly ISystemAccountRepository systemAccountRepository;
        private readonly IHubContext<SignalRHub> hubContext;


        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public ManageAccountsModel(IHubContext<SignalRHub> hubContext)
        {
            systemAccountRepository = new SystemAccountRepository();
            this.hubContext = hubContext;

        }

        public List<SystemAccount> Accounts { get; set; }

        [BindProperty]
        public short AccountId { get; set; }

        public void OnGet(string searchTerm)
        {
            // Thực hiện tìm kiếm nếu SearchTerm không rỗng
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Accounts = systemAccountRepository.SearchSystemAccounts(SearchTerm);
            }
            else
            {
                Accounts = systemAccountRepository.GetSystemAccounts();

            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (AccountId > 0)
            {
                var account = systemAccountRepository.GetSystemAccountById(AccountId);
                if (account != null)
                {
                    systemAccountRepository.DeleteAccount(account);
                }
            }

            await hubContext.Clients.All.SendAsync("RefreshData");

            return RedirectToPage("/Admin/ManageAccounts"); // Redirect back to the ManageAccounts page
        }
    }
}

