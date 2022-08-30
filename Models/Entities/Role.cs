using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElSaiys.Models.Entities
{
    public class Role
    {
        [Required]
        public string Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
    }
}
