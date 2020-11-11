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
    public class IndexModel : PageModel
    {
        private readonly OtterDbContext _context;

        public IndexModel(OtterDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Otter> Vydra { get; set; }

        public void OnGet()
        {
            Vydra = _context.Otters
                .Include(v => v.Place.Location)
                .Include(v => v.Mother)
                .Include(v => v.Place)
                .Include(v => v.Founder).AsNoTracking().AsEnumerable();
        }
    }
}