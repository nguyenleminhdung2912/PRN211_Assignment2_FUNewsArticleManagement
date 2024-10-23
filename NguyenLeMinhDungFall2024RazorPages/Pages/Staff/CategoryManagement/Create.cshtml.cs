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

namespace NguyenLeMinhDungFall2024RazorPages.Pages.Staff.CategoryManagement
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryRepository categoryRepository;

        public CreateModel()
        {
            categoryRepository = new CategoryRepository();
        }

        public IActionResult OnGet()
        {
        ViewData["ParentCategoryId"] = new SelectList(categoryRepository.GetCategories(), "CategoryId", "CategoryDesciption");
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            categoryRepository.SaveCategory(Category);


            return RedirectToPage("./Index");
        }
    }
}
