using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElSaiys.DTOs
{
    public class MailRequest
    {
        [Required]
        public string ToEmail { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string body { get; set; }
        public IList<IFormFile> Attachments { get; set; }
    }
}
