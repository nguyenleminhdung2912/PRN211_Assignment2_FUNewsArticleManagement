using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using DataAccessObjects;
using Repository.IRepository;
using Repository.Repository;

namespace NguyenLeMinhDungFall2024RazorPages.Pages.Admin
{
    public class AdminEditUserModel : PageModel
    {
        private readonly ISystemAccountRepository systemAccountRepository;

        public AdminEditUserModel()
        {
            systemAccountRepository = new SystemAccountRepository();
        }

        [BindProperty]
        public SystemAccount SystemAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SystemAccount systemaccount = systemAccountRepository.GetSystemAccountById(id);
            if (systemaccount == null)
            {
                return NotFound();
            }
            SystemAccount = systemaccount;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                systemAccountRepository.SaveAccount(SystemAccount);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (SystemAccountExists(SystemAccount.AccountId) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private SystemAccount SystemAccountExists(short id)
        {
            return systemAccountRepository.GetSystemAccountById(id);
        }
    }
}
