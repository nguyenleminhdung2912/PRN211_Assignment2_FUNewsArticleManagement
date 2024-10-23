
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using DataAccessObjects;
using Repository.IRepository;
using Repository.Repository;

namespace NguyenLeMinhDungFall2024RazorPages.Pages.Staff.CategoryManagement
{
    public class IndexModel : PageModel
    {
        private ICategoryRepository categoryRepository;

        public IndexModel()
        {
            categoryRepository = new CategoryRepository();
        }

        public IList<Category> Category { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Category = categoryRepository.GetCategories();
        }
    }
}
