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
    public class DeleteItemModel : PageModel
    {
        private readonly OtterDbContext _context;

        public DeleteItemModel(OtterDbContext context)
        {
            _context = context;
        }

        public string GetUserId()
        {
            return HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? default;
        }

        [BindProperty]
        public Otter Otter { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Otter = await _context.Otters
                .Include(o => o.Founder)
                .Include(o => o.Mother)
                .Include(o => o.Place.Location)
                .Include(o => o.Place).FirstOrDefaultAsync(m => m.TattooID == id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            Otter = _context.Otters.Find(id);
            if (GetUserId() == Otter.FounderId)
            {
                if (Otter != null)
                {
                    //foreach (var item in _context.Otters)
                    //{
                    //    if (_context.Otters.Find(Otter.Mother.MotherId))
                    //    {

                    //    }
                    //}
                    _context.Otters.Remove(Otter);
                    await _context.SaveChangesAsync();
                }
                return RedirectToPage("../Index");
            }
            else
            {
                return RedirectToPage("./Denied");
            }
        }
    }
}
