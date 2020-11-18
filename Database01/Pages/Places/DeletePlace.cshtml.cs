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
    public class DeletePlaceModel : PageModel
    {
        private readonly OtterDbContext _context;

        public DeletePlaceModel(OtterDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Place place { get; set; }

        public async Task<IActionResult> OnGetAsync(string id, int? idLoc)
        {
            place = await _context.Places.Include(p => p.Location).AsNoTracking().FirstOrDefaultAsync(m => m.Name == id && m.LocationId == idLoc);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            foreach (var item in _context.Otters)
            {
                if (item.PlaceName == place.Name && item.LocationId == place.LocationId)
                {
                    return RedirectToPage("../Logged/Denied");
                }
            }

            _context.Places.Remove(place);
            await _context.SaveChangesAsync();

            return RedirectToPage("./PlacesIndex");

        }
    }
}
