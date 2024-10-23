using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;

namespace RazorPage.Pages
{
    public class IndexModel : PageModel
    {
        private readonly BusinessObjects.FunewsManagementFall2024Context _context;

        public IndexModel(BusinessObjects.FunewsManagementFall2024Context context)
        {
            _context = context;
        }

        public IList<Category> Category { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Category = await _context.Categories
                .Include(c => c.ParentCategory).ToListAsync();
        }
    }
}
