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
    public class EditLocationModel : PageModel
    {
        private readonly OtterDbContext _context;
        [BindProperty]
        public Location location { get; set; }
        public EditLocationModel(OtterDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            location = await _context.Locations.AsNoTracking().FirstOrDefaultAsync(m => m.LocationID == id);


            if (location == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _context.Attach(location).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(location.LocationID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./LocationIndex");
        }

        private bool LocationExists(int? id)
        {
            return _context.Locations.Any(e => e.LocationID == id);
        }
    }
}