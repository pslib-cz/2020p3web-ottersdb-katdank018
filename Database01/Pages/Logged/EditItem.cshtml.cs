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
    public class EditItemModel : PageModel
    {
        private readonly OtterDbContext _context;

        public EditItemModel(OtterDbContext context)
        {
            _context = context;
        }


        [BindProperty]
        public Otter otter { get; set; }
        [BindProperty]
        public Place Place { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            otter = await _context.Otters
                .Include(v => v.Place.Location)
                .Include(v => v.Mother)
                .Include(v => v.Place).AsNoTracking().FirstOrDefaultAsync(m => m.TattooID == id);


            if (otter == null)
            {
                return NotFound();
            }

            PlaceNames = new List<SelectListItem>();
            Mothers = new List<SelectListItem>();
            Mothers.Add(new SelectListItem("", null));
            foreach (var item in _context.Places.Include(l => l.Location).AsEnumerable<Place>())
            {
                if (otter.PlaceName == item.Name && otter.LocationId == item.LocationId)
                {
                    PlaceNames.Add(new SelectListItem($"{item.Name} ({item.Location.Name})", $"{item.LocationId};{item.Name}", selected: true));
                }
                else
                {
                    PlaceNames.Add(new SelectListItem($"{item.Name} ({item.Location.Name})", $"{item.LocationId};{item.Name}"));
                }
            }

            foreach (var item in _context.Otters.Include(l => l.Mother).AsEnumerable<Otter>())
            {
                if (otter.Mother?.TattooID == item.TattooID)
                {
                    Mothers.Add(new SelectListItem($"{item.Name}", $"{item.TattooID}", selected:true));
                }
                else
                {
                    Mothers.Add(new SelectListItem($"{item.Name}", $"{item.TattooID}", selected: false));
                }

            }
            return Page();
        }

        public List<SelectListItem> PlaceNames { get; set; }
        public List<SelectListItem> Mothers { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            string[] data;
            data = otter.PlaceName.Split(';');
            otter.LocationId = int.Parse(data[0]);
            otter.PlaceName = data[1];
            _context.Attach(otter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VydraExists(otter.TattooID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("../Index");
        }

        private bool VydraExists(int? id)
        {
            return _context.Otters.Any(e => e.TattooID == id);
        }
    }
}