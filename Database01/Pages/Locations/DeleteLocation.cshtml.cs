﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Database01.Model;

namespace Database01.Pages
{
    public class DeleteLocationModel : PageModel
    {
        private readonly OtterDbContext _context;

        public DeleteLocationModel(OtterDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Location location { get; set; }
        public string Denied { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            location = await _context.Locations.FirstOrDefaultAsync(m => m.LocationID == id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            foreach (var item in _context.Otters)
            {
                if (item.LocationId == location.LocationID)
                {
                    Denied = "V této lokaci žije nějaká vydra.";
                    return Page();
                }
            }
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
            return RedirectToPage("../LocationIndex");

        }
    }
}
