using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Database01.Model;

namespace Database01.Pages
{
    public class CreateItemModel : PageModel
    {
        private readonly OtterDbContext _context;

        public CreateItemModel(Database01.Model.OtterDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["FounderId"] = new SelectList(_context.Users, "Id", "Id");
        ViewData["MotherId"] = new SelectList(_context.Otters, "TattooID", "TattooID");
        ViewData["PlaceName"] = new SelectList(_context.Places, "Name", "Name");
            return Page();
        }

        [BindProperty]
        public Otter Otter { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Otters.Add(Otter);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
