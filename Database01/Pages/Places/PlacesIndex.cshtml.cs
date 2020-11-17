using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Database01.Model;
using Microsoft.AspNetCore.Identity;


namespace Database01.Pages
{
    public class PlacesIndexModel : PageModel
    {
        private readonly OtterDbContext _context;

        public PlacesIndexModel(OtterDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Place> places { get; set; }

        public void OnGet()
        {
            places = _context.Places
                .Include(v => v.Location).AsNoTracking().AsEnumerable();
        }
    }
}