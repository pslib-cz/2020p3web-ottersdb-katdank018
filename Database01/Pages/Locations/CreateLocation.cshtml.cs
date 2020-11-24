using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Database01.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Database01.Pages
{
    public class CreateLocationModel : PageModel
    {
        private readonly OtterDbContext _context;
        [BindProperty]
        public Location location { get; set; }
        public CreateLocationModel(OtterDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return RedirectToPage("../LocationIndex");
        }
    }
}
