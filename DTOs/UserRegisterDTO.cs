using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElSaiys.DTOs
{
    public class UserRegisterDTO
    {
        [Required, StringLength(25)]
        public string FirstName { get; set; }

        [Required, StringLength(25)]
        public string LastName { get; set; }

        [Required, StringLength(25)]
        public string Username { get; set; }
        public string PhoneNumber { get; set; }

        [Required, EmailAddress, StringLength(50)]
        public string Email { get; set; }

        public bool IsPrivate { get; set; }
        public bool IsDelete { get; set; }

        [Required, StringLength(50)]
        public string Password { get; set; }

        public string RoleId { get; set; }
    }
}
