using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ElSaiys.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElSaiys.DTOs;

namespace ElSaiys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailingSrvice _mailingService;

        public MailController(IMailingSrvice mailingService)
        {
            _mailingService = mailingService;
        }

        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail([FromForm] MailRequest dto)
        {
            await _mailingService.SendEmailAsync(dto.ToEmail, dto.Subject, dto.body, dto.Attachments);
            return Ok();
        }
    }
}
