using ElSaiys.DTOs;
using ElSaiys.Helper;
using ElSaiys.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElSaiys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuhController : ControllerBase
    {
        private readonly IAuthRepository _userRrpo;
        private readonly IError _error;

        public AuhController(IAuthRepository userRrpo, IError error)
        {
            _userRrpo = userRrpo;
            _error = error;
        }

        [HttpPost("Registeation")]
        public async Task<IActionResult> Registeation(UserRegisterDTO userRegisterDTO)
        {
            var authResault = await _userRrpo.Registration(userRegisterDTO);

            if (authResault.Success)
                return Ok(authResault);

            _error.LoadError(authResault.ErrorCode);
            ModelState.AddModelError(_error.ErrorProp, _error.ErrorMessage);
            return ValidationProblem();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
        {
            var authResault = await _userRrpo.Login(userLoginDTO);

            if (authResault.Success)
                return Ok(authResault);

            _error.LoadError(authResault.ErrorCode);
            ModelState.AddModelError(_error.ErrorProp, _error.ErrorMessage);
            return ValidationProblem();
        }
    }
}
