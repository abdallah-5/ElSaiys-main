using ElSaiys.DTOs;
using ElSaiys.Helper;
using ElSaiys.Repositories;
using ElSaiys.Services;
using ElSaiys.Services.Interfaces;
using ElSaiys.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElSaiys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRrpo;
        private readonly IError _error;
        private readonly IMailingSrvice _mailingService;

        public UsersController(IUserRepository userRrpo, IError error, IMailingSrvice mailingService)
        {
            _userRrpo = userRrpo;
            _error = error;
            _mailingService = mailingService;
        }

        [HttpGet("slug")]
        [Authorize]
        public async Task<IActionResult> WhoseCart(string slug)
        {
            var userResault = await _userRrpo.CarOwner(slug);

            if (userResault.Success)
                return Ok(userResault);

            _error.LoadError(userResault.ErrorCode);
            ModelState.AddModelError(_error.ErrorProp, _error.ErrorMessage);
            return ValidationProblem();
        }

        [HttpGet("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var userResault = await _userRrpo.UserExist(email);

            if (!userResault.Success)
            {
                _error.LoadError(userResault.ErrorCode);
                ModelState.AddModelError(_error.ErrorProp, _error.ErrorMessage);
                return ValidationProblem();
            }

            var verificationCode = _userRrpo.GenrateVerificationCode(userResault);

            var mail = new MailController(_mailingService);
            await mail.SendMail(new MailRequest
            {
                ToEmail = userResault.Email,
                Subject = "Verification Code",
                body = verificationCode.ToString()
            });

            return Ok();
        }

        [HttpGet("VerificationCode")]
        public async Task<IActionResult> CheckVerificationCode(string email, int verificationCode)
        {
            var verificationCodeExist = await _userRrpo.CheckVerificationCode(email, verificationCode);

            if (!verificationCodeExist.Success)
            {
                _error.LoadError(verificationCodeExist.ErrorCode);
                ModelState.AddModelError(_error.ErrorProp, _error.ErrorMessage);
                return ValidationProblem();
            }

            return Ok();
        }

        [HttpGet("ReInsertPassword")]
        public IActionResult UpdatePassword(string email, string password)
        {
            _userRrpo.UpdatePassword(email, password);

            return Ok();
        }
    }
}
