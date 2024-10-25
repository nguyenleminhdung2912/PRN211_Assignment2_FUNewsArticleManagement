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
using Microsoft.AspNetCore.SignalR;

namespace NguyenLeMinhDungFall2024RazorPages.Pages.Staff.CategoryManagement
{
    public class EditModel : PageModel
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IHubContext<SignalRHub> hubContext;


        public EditModel(IHubContext<SignalRHub> hubContext)
        {
            categoryRepository = new CategoryRepository(); 
            this.hubContext = hubContext;

        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            Category = category;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var category = categoryRepository.GetCategoryById(Category.CategoryId);
                Category.ParentCategory = category.ParentCategory;
                Category.ParentCategoryId = category.ParentCategoryId;
                Category.IsActive = category.IsActive;
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

            await hubContext.Clients.All.SendAsync("RefreshData");

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
