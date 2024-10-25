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
using Microsoft.AspNetCore.SignalR;

namespace NguyenLeMinhDungFall2024RazorPages.Pages.Staff.CategoryManagement
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IHubContext<SignalRHub> hubContext;

        public CreateModel(IHubContext<SignalRHub> hubContext)
        {
            categoryRepository = new CategoryRepository();
            this.hubContext = hubContext;
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

            Category.IsActive = true;
            categoryRepository.SaveCategory(Category);

            // Gọi RefreshData
            await hubContext.Clients.All.SendAsync("RefreshData");

            return RedirectToPage("./Index");
        }
    }
}
