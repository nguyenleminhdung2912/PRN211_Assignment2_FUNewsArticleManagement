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
using Microsoft.AspNetCore.SignalR;

namespace NguyenLeMinhDungFall2024RazorPages.Pages.Staff.CategoryManagement
{
    public class DetailsModel : PageModel
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IHubContext<SignalRHub> hubContext;


        public DetailsModel(IHubContext<SignalRHub> hubContext)
        {
            categoryRepository = new CategoryRepository();
            this.hubContext = hubContext;

        }

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
            else
            {
                Category = category;
            }
            await hubContext.Clients.All.SendAsync("RefreshData");

            return Page();
        }
    }
}
