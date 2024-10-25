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
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace NguyenLeMinhDungFall2024RazorPages.Pages.Staff.NewsManagement
{
    public class CreateModel : PageModel
    {
        private readonly INewsArticleRepository newsArticleRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ISystemAccountRepository systemAccountRepository;
        private readonly ITagRepository tagRepository;
        private readonly IHubContext<SignalRHub> hubContext;


        public CreateModel(IHubContext<SignalRHub> hubContext)
        {
            newsArticleRepository = new NewsArticleRepository();
            categoryRepository = new CategoryRepository();
            systemAccountRepository = new SystemAccountRepository();
            tagRepository = new TagRepository();
            this.hubContext = hubContext;

        }


        //[BindProperty]
        //public List<Tag> AvailableTags { get; set; } = new List<Tag>();

        //[BindProperty]
        //public List<int> SelectedTagIds { get; set; } = new List<int>();

        public async Task<IActionResult> OnGet()
        {
            // Load available tags
            //var tags = tagRepository.GetTags();
            //AvailableTags = tags;
            ViewData["CategoryId"] = new SelectList(categoryRepository.GetCategories(), "CategoryId", "CategoryName");
            ViewData["CreatedById"] = new SelectList(systemAccountRepository.GetSystemAccounts(), "AccountId", "AccountId");
            //ViewData["Tags"] = new SelectList(AvailableTags, "TagId", "TagName");
            return Page();
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var AccountEmail = User.FindFirst(ClaimTypes.Name)?.Value;
                SystemAccount systemAccount = systemAccountRepository.GetSystemAccountByEmail(AccountEmail);

                //List<Tag> tags = new List<Tag>();
                //foreach (var id in SelectedTagIds)
                //{
                //    Tag tag1 = tagRepository.GetTagById(id);
                //    tags.Add(tag1);
                //}

                Category category = categoryRepository.GetCategoryById(NewsArticle.CategoryId);

                NewsArticle.CreatedDate = DateTime.Now;
                NewsArticle.CreatedBy = systemAccount;
                NewsArticle.CreatedById = systemAccount.AccountId;
                NewsArticle.NewsStatus = true;
                NewsArticle.ModifiedDate = DateTime.Now;
                NewsArticle.Category = category;

                //NewsArticle.Tags = tags;

                await hubContext.Clients.All.SendAsync("RefreshData");


                newsArticleRepository.SaveNewsArticle(NewsArticle);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsArticleExists(NewsArticle.NewsArticleId))
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
        private bool NewsArticleExists(string id)
        {
            return newsArticleRepository.GetNewsArticleById(id) == null ? true : false;
        }
    }
}
