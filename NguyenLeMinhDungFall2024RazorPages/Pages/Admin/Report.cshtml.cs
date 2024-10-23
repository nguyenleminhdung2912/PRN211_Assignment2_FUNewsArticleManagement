using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository.IRepository;
using Repository.Repository;

namespace NguyenLeMinhDungFall2024RazorPages.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ReportModel : PageModel
    {
        private readonly INewsArticleRepository newsArticleRepository;

        [BindProperty]
        public DateTime StartDate { get; set; } = new DateTime(2024, 1, 1);

        [BindProperty]
        public DateTime EndDate { get; set; } = new DateTime(2025, 1, 1);

        public bool ReportGenerated { get; set; }

        public int TotalNewsArticles { get; set; }

        public List<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();

        public ReportModel() 
        {
            newsArticleRepository = new NewsArticleRepository();
        }

        public void OnPost()
        {
            // Giả sử đây là thống kê lấy từ database
            // Lấy danh sách bài viết tin tức
            NewsArticles = newsArticleRepository.GetNewsArticlesByDateRange(StartDate, EndDate);
            TotalNewsArticles = NewsArticles.Count;
            ReportGenerated = true;
        }
    }
}
