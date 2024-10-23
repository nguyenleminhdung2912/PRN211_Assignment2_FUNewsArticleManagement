using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using Repository.IRepository;
using Repository.Repository;
using System.Security.Claims;

namespace NguyenLeMinhDungFall2024RazorPages.Pages.Staff.NewsManagement
{
    public class EditModel : PageModel
    {
        private readonly INewsArticleRepository newsArticleRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ISystemAccountRepository systemAccountRepository;
        private readonly ITagRepository tagRepository;

        public EditModel()
        {
            newsArticleRepository = new NewsArticleRepository();
            categoryRepository = new CategoryRepository();
            systemAccountRepository = new SystemAccountRepository();
            tagRepository = new TagRepository();
        }

        [BindProperty]
        public NewsArticle NewsArticle { get; set; } = default!;

        [BindProperty]
        public List<Tag> AvailableTags { get; set; } = new List<Tag>();

        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new List<int>();


        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsarticle = newsArticleRepository.GetNewsArticleById(id);
            if (newsarticle == null)
            {
                return NotFound();
            }
            NewsArticle = newsarticle;

            // Load available tags
            var tags = tagRepository.GetTags();
            AvailableTags = tags;

            // Load selected tags for this news article
            SelectedTagIds = newsarticle.Tags.Select(t => t.TagId).ToList();

            ViewData["CategoryId"] = new SelectList(categoryRepository.GetCategories(), "CategoryId", "CategoryName");
           ViewData["CreatedById"] = new SelectList(systemAccountRepository.GetSystemAccounts(), "AccountId", "AccountId");
           ViewData["Tags"] = new SelectList(AvailableTags, "TagId", "TagName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Lấy AccountEmail từ claims
                var AccountEmail = User.FindFirst(ClaimTypes.Name)?.Value;

                NewsArticle currentNews = newsArticleRepository.GetNewsArticleById(NewsArticle.NewsArticleId);

                NewsArticle.CreatedDate = currentNews.CreatedDate;
                NewsArticle.ModifiedDate = DateTime.Now;
                NewsArticle.UpdatedById = systemAccountRepository.GetSystemAccountByEmail(AccountEmail).AccountId;
                NewsArticle.NewsStatus = currentNews.NewsStatus;
                NewsArticle.CreatedBy = currentNews.CreatedBy;
                NewsArticle.CreatedById = currentNews.CreatedById;
                NewsArticle.Category = categoryRepository.GetCategoryById(NewsArticle.CategoryId);
                // Associate selected tags
                List<Tag> tags = new List<Tag>();
                foreach (var id in SelectedTagIds)
                {
                    Tag tag1 = tagRepository.GetTagById(id);
                    tags.Add(tag1);
                }

                NewsArticle.Tags = tags;

                newsArticleRepository.UpdateNewsArticle(NewsArticle, SelectedTagIds);
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
