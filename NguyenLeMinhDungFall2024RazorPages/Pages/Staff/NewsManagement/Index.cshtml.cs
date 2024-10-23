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

namespace NguyenLeMinhDungFall2024RazorPages.Pages.Staff.NewsManagement
{
    public class IndexModel : PageModel
    {
        private readonly INewsArticleRepository newsArticleRepository;

        public IndexModel()
        {
            newsArticleRepository = new NewsArticleRepository();
        }

        public IList<NewsArticle> NewsArticle { get;set; } = default!;

        public async Task OnGetAsync()
        {
            NewsArticle = newsArticleRepository.GetNewsArticles();
        }
    }
}
