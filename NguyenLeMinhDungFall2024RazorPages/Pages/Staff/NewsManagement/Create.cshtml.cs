using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects;
using DataAccessObjects;
using Repository.IRepository;
using Repository.Repository;

namespace NguyenLeMinhDungFall2024RazorPages.Pages.Staff.NewsManagement
{
    public class CreateModel : PageModel
    {
        private readonly INewsArticleRepository newsArticleRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ISystemAccountRepository systemAccountRepository;

        public CreateModel()
        {
            newsArticleRepository = new NewsArticleRepository();
            categoryRepository = new CategoryRepository();
            systemAccountRepository = new SystemAccountRepository();
        }

        public IActionResult OnGet()
        {
        ViewData["CategoryId"] = new SelectList(categoryRepository.GetCategories(), "CategoryId", "CategoryDesciption");
        ViewData["CreatedById"] = new SelectList(systemAccountRepository.GetSystemAccounts(), "AccountId", "AccountId");
            return Page();
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            newsArticleRepository.SaveNewsArticle(NewsArticle);

            return RedirectToPage("./Index");
        }
    }
}
