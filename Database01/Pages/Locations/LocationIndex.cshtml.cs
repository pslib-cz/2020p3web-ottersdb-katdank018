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
    public class LocationIndexModel : PageModel
    {
        private readonly OtterDbContext _context;

        public LocationIndexModel(OtterDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Location> location { get; set; }

        public void OnGet()
        {
            location = _context.Locations;
        }
    }
}