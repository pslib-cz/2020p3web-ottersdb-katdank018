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
    public class CreateItemModel : PageModel
    {
        private readonly OtterDbContext _context;

        [BindProperty]
        public Otter Otter { get; set; }
        [BindProperty]
        public Place Place { get; set; }

        public List<SelectListItem> PlaceNames { get; set; }
        public List<SelectListItem> Mothers { get; set; }

        public CreateItemModel(Database01.Model.OtterDbContext context)
        {
            _context = context;
        }

        public string GetUserId()
        {
            return HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? default;
        }

        public IActionResult OnGet()
        {

            PlaceNames = new List<SelectListItem>();
            Mothers = new List<SelectListItem>();
            Mothers.Add(new SelectListItem("",null));
            foreach (var item in _context.Places.Include(l => l.Location).AsEnumerable<Place>())
            {
                PlaceNames.Add(new SelectListItem($"{item.Name} ({item.Location.Name})",$"{item.LocationId};{item.Name}"));
            }
            foreach (var item in _context.Otters.Include(l => l.Mother).AsEnumerable<Otter>())
            {
                Mothers.Add(new SelectListItem($"{item.Name}", $"{item.TattooID}"));
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Otter.FounderId = GetUserId();

            string[] data;
            data = Otter.PlaceName.Split(';');
            Otter.LocationId = int.Parse(data[0]);
            Otter.PlaceName = data[1];
            _context.Otters.Add(Otter);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Index");
        }
    }
}
