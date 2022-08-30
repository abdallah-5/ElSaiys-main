using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElSaiys.DTOs
{
    public class UserResult
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
    }
}
