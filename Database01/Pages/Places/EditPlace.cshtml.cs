using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Database01.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Database01.Pages
{
    public class EditPlaceModel : PageModel
    {
        private readonly OtterDbContext _context;
        [BindProperty]
        public Place place { get; set; }
        public EditPlaceModel(OtterDbContext context)
        {
            _context = context;
        }
        public List<SelectListItem> Locations { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            place = await _context.Places.Include(v => v.Location).AsNoTracking().FirstOrDefaultAsync(m => m.LocationId == id);


            if (place == null)
            {
                return NotFound();
            }

            Locations = new List<SelectListItem>();
            foreach (var item in _context.Locations)
            {
                Locations.Add(new SelectListItem($"{item.Name}", $"{item.LocationID}"));
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _context.Attach(place).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlaceExists(place.LocationId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./PlacesIndex");
        }

        private bool PlaceExists(int? id)
        {
            return _context.Places.Any(e => e.LocationId == id);
        }
    }
}