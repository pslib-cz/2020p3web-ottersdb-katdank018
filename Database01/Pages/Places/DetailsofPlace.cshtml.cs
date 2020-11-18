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
    public class DetailsofPlaceModel : PageModel
    {
        private readonly OtterDbContext _context;

        public DetailsofPlaceModel(OtterDbContext context)
        {
            _context = context;
        }

        public Place place { get; set; }

        public async Task<IActionResult> OnGetAsync(string id, int? idLoc)
        {
            place = await _context.Places.Include(l => l.Location).FirstOrDefaultAsync(m => m.Name == id && m.LocationId == idLoc);

            if (place == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
