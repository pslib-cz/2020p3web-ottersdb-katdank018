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
    public class DeleteLocationModel : PageModel
    {
        private readonly OtterDbContext _context;

        public DeleteLocationModel(OtterDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Location location { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            location = await _context.Locations.FirstOrDefaultAsync(m => m.LocationID == id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            location = _context.Locations.Find(id);
                if (location != null)
                {
                    foreach (var item in _context.Otters)
                    {
                        if (item.MotherId == location.LocationID)
                        {
                            item.MotherId = null;
                        }
                    }
                    _context.Locations.Remove(location);
                    await _context.SaveChangesAsync();
                }
                return RedirectToPage("./LocationIndex");

        }
    }
}
