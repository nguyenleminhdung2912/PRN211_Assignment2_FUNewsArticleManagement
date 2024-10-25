
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using DataAccessObjects;
using Repository.IRepository;
using Repository.Repository;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<IActionResult> OnPostDeleteAsync(int categoryId)
        {
            short id = (short)categoryId;
            var category = categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            if (category.NewsArticles != null && category.NewsArticles.Any())
            {
                // Thêm thông báo lỗi vào ModelState
                ModelState.AddModelError(string.Empty, "Category đang được sử dụng, không thể xoá.");
                // Tải lại dữ liệu để hiển thị danh sách cập nhật và giữ nguyên trạng thái của trang
                await OnGetAsync();
                return Page();
            }
            category.IsActive = false;
            categoryRepository.UpdateCategory(category);

            // Sau khi xóa, tải lại dữ liệu để hiển thị danh sách cập nhật
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostReActivateAsync(int categoryId)
        {
            short id = (short)categoryId;
            var category = categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            // Kích hoạt lại danh mục
            category.IsActive = true;
            categoryRepository.UpdateCategory(category);

            // Tải lại trang để cập nhật danh sách sau khi kích hoạt lại
            return RedirectToPage();
        }
    }
}
