using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Database01.Model;

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
            location = await _context.Locations.AsNoTracking().FirstOrDefaultAsync(m => m.LocationID == id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _context.Attach(location).State = EntityState.Modified;
            await _context.SaveChangesAsync();


            return RedirectToPage("./LocationIndex");
        }
    }
}