using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;
using Repository.Repository;

namespace NguyenLeMinhDungFall2024RazorPages.Pages.Lecturer
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleRepository newsArticleRepository; // Thay bằng DbContext của bạn

        public IndexModel()
        {
            newsArticleRepository = new NewsArticleRepository();
            NewsArticles = new List<NewsArticle>();
            SelectedArticle = null; // Initialize as null
            NewsArticles = newsArticleRepository.GetNewsArticles();
        }

        public IList<NewsArticle> NewsArticles { get; set; }
        public NewsArticle? SelectedArticle { get; set; }

        public void OnGet()
        {
            NewsArticles = newsArticleRepository.GetNewsArticles();
        }

        public IActionResult OnGetDetail(string id)
        {
            SelectedArticle = newsArticleRepository.GetNewsArticleById(id);
            if (SelectedArticle == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
