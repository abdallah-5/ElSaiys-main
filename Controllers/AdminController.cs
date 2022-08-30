using ElSaiys.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElSaiys.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRrpo;

        public AdminController(IAdminRepository adminRrpo)
        {
            _adminRrpo = adminRrpo;
        }

        [HttpGet("AllUsers")]
        [Authorize]
        public async Task<IActionResult> AllUsers()
        {
            return Ok(await _adminRrpo.AllUsers());
        }

        [HttpPost("DisableUser")]
        [Authorize]
        public IActionResult DisableUser(string userId)
        {
            _adminRrpo.DisableUser(userId);
            return Ok();
        }

        [HttpPost("UnDisableUser")]
        [Authorize]
        public IActionResult UnDisableUser(string userId)
        {
            _adminRrpo.UnDisableUser(userId);
            return Ok();
        }

        [HttpGet("TrashUsers")]
        [Authorize]
        public async Task<IActionResult> TrashUsers()
        {
            return Ok(await _adminRrpo.TrashUsers());
        }

        [HttpDelete("DeleteUser")]
        [Authorize]
        public IActionResult DeleteUser(string userId)
        {
            _adminRrpo.DeleteUser(userId);
            return Ok();
        }
    }
}
