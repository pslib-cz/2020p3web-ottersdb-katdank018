using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Database01.Model;

namespace Database01.Pages
{
    public class DetailsofLocationModel : PageModel
    {
        private readonly OtterDbContext _context;

        public DetailsofLocationModel(OtterDbContext context)
        {
            _context = context;
        }

        public Location location { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            location = await _context.Locations.FirstOrDefaultAsync(m => m.LocationID == id);

            if (location == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
