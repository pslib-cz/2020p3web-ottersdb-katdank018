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
    public class DetailsofItemModel : PageModel
    {
        private readonly OtterDbContext _context;

        public DetailsofItemModel(OtterDbContext context)
        {
            _context = context;
        }

        public Otter Otter { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Otter = await _context.Otters
                .Include(o => o.Founder)
                .Include(o => o.Mother)
                .Include(o => o.Place.Location)
                .Include(o => o.Place).FirstOrDefaultAsync(m => m.TattooID == id);

            if (Otter == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
