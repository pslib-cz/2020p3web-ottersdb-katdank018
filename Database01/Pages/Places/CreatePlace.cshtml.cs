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
    public class CreatePlaceModel : PageModel
    {
        private readonly OtterDbContext _context;
        [BindProperty]
        public Place place { get; set; }

        public List<SelectListItem> Locations { get; set; }
        public CreatePlaceModel(Database01.Model.OtterDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            Locations = new List<SelectListItem>();
            foreach (var item in _context.Locations)
            {
                Locations.Add(new SelectListItem($"{item.Name}", $"{item.LocationID}"));
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _context.Places.Add(place);
            await _context.SaveChangesAsync();

            return RedirectToPage("./PlacesIndex");
        }
    }
}
