using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database01.Model;

namespace Database01.Pages
{
    public class EditPlaceModel : PageModel
    {
        private readonly OtterDbContext _context;

        public EditPlaceModel(OtterDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Place Edit { get; set; }
        [BindProperty]
        public Place place { get; set; }

        public List<SelectListItem> Locations { get; set; }
        public async Task<IActionResult> OnGetAsync(string id, int? idLoc)
        {
            place = await _context.Places
                .Include(p => p.Location).AsNoTracking().FirstOrDefaultAsync(m => m.Name == id && m.LocationId == idLoc);

            Locations = new List<SelectListItem>();
            foreach (var item in _context.Locations)
            {
                Locations.Add(new SelectListItem($"{item.Name}", $"{item.LocationID}"));
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Place NewPlace = place;
            _context.Places.Remove(place);
            await _context.SaveChangesAsync();
            NewPlace.LocationId = Edit.LocationId;
            NewPlace.Name = Edit.Name;
            _context.Places.Add(NewPlace);

            await _context.SaveChangesAsync();
            return RedirectToPage("./PlacesIndex");
        }
    }
}