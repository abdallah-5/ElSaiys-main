using ElSaiys.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElSaiys.Models
{
    public class User
    {
        [Required]
        public string Id { get; set; }

        [Required, MaxLength(25)]
        public string FirstName { get; set; }

        [Required, MaxLength(25)]
        public string LastName { get; set; }

        [Required, MaxLength(25)]
        public string Username { get; set; }

        [Required, MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required, EmailAddress, MaxLength(50)]
        public string Email { get; set; }

        public bool IsPrivate { get; set; }
        public bool IsDelete { get; set; }

        [Required, MaxLength(256)]
        public byte[] Passwordhash { get; set; }

        [Required, MaxLength(256)]
        public byte[] Passwordsalt { get; set; }

        [Required, MaxLength(50)]
        public string Slug { get; set; }

        [MaxLength(7)]
        public int VerificationCode { get; set; }

        [Required]
        public string RoleId { get; set; }
        public Role Role { get; set; }
    }
}
