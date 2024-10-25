using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using DataAccessObjects;
using Repository.IRepository;
using Repository.Repository;

namespace NguyenLeMinhDungFall2024RazorPages.Pages.Staff.ProfileManagement
{
    public class ProfileModel : PageModel
    {
        private readonly ISystemAccountRepository systemAccountRepository;
        private readonly INewsArticleRepository newsArticleRepository;

        public ProfileModel()
        {
            systemAccountRepository = new SystemAccountRepository();
            newsArticleRepository = new NewsArticleRepository();
        }

        public SystemAccount SystemAccount { get; set; } = default!;

        public bool HistoryPostGenerated { get; set; }

        public int TotalNewsArticles { get; set; }

        public List<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();

        public async Task<IActionResult> OnGetAsync()
        {
            string Email = User.Identity.Name;

            var systemaccount = systemAccountRepository.GetSystemAccountByEmail(Email);
            if (systemaccount == null)
            {
                return NotFound();
            }
            else
            {
                SystemAccount = systemaccount;
                NewsArticles = newsArticleRepository.GetNewsArticlesCreatedBy(SystemAccount.AccountId);
                TotalNewsArticles = NewsArticles.Count;
                HistoryPostGenerated = true;
            }
            return Page();
        }
    }
}
