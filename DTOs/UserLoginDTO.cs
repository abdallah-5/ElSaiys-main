using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElSaiys.DTOs
{
    public class UserLoginDTO
    {
        [Required, StringLength(25)]
        public string Username { get; set; }

        [Required, StringLength(50)]
        public string Password { get; set; }
    }
}
