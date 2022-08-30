using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElSaiys.DTOs
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string ErrorCode { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsPrivate { get; set; }
        public string Slug { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
