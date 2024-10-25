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

namespace NguyenLeMinhDungFall2024RazorPages.Pages.Staff.ProfileManagement
{
    public class EditProfileModel : PageModel
    {
        private readonly ISystemAccountRepository systemAccountRepository;

        public EditProfileModel()
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

            var systemaccount = systemAccountRepository.GetSystemAccountById(id);
            if (systemaccount == null)
            {
                return NotFound();
            }
            SystemAccount = systemaccount;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var systemaccount = systemAccountRepository.GetSystemAccountById(SystemAccount.AccountId);
                SystemAccount.AccountRole = systemaccount.AccountRole;
                systemAccountRepository.UpdateAccount(SystemAccount);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SystemAccountExists(SystemAccount.AccountId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Profile");
        }

        private bool SystemAccountExists(short id)
        {
            return systemAccountRepository.GetSystemAccountById(id) == null ? true : false;
        }
    }
}
