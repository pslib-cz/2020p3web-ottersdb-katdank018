using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Database01.Model
{
    public class Otter
    {
        public IdentityUser Founder { get; set; }
        [ForeignKey("Founder")]
        public string FounderId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        [Key]
        public int? TattooID { get; set; }
        public Otter Mother { get; set; }

        [ForeignKey("Mother")]
        public int? MotherId { get; set; }

        [Required]
        public Place Place { get; set; }

        public string PlaceName { get; set; }

        public int LocationId { get; set; }

        public ICollection<Otter> Children { get; set; }
    }
}