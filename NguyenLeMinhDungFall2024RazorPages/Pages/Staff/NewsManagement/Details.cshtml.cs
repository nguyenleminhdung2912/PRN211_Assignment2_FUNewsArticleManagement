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
    public class DetailsModel : PageModel
    {
        private readonly INewsArticleRepository newsArticleRepository;

        public DetailsModel()
        {
            newsArticleRepository = new NewsArticleRepository();
        }

        public NewsArticle NewsArticle { get; set; } = default!;

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
            else
            {
                NewsArticle = newsarticle;
            }
            return Page();
        }
    }
}
