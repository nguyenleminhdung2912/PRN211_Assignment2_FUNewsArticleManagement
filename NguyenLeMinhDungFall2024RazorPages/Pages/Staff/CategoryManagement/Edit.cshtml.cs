using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using DataAccessObjects;
using Repository.IRepository;
using Repository.Repository;

namespace NguyenLeMinhDungFall2024RazorPages.Pages.Staff.CategoryManagement
{
    public class EditModel : PageModel
    {
        private readonly ICategoryRepository categoryRepository;

        public EditModel()
        {
            categoryRepository = new CategoryRepository();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category =  categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            Category = category;
           ViewData["ParentCategoryId"] = new SelectList(categoryRepository.GetCategories(), "CategoryId", "CategoryDesciption");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                categoryRepository.UpdateCategory(Category);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(Category.CategoryId))
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

        private bool CategoryExists(short id)
        {
            if (categoryRepository.GetCategoryById(id) != null)
            {
                return true;
            }
            return false;
        }
    }
}
