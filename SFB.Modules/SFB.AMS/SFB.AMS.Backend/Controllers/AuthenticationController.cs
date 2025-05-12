﻿using Microsoft.AspNetCore.Mvc;
using SFB.Shared.Backend.Controller;
using SFB.AMS.Shared.Models;
using SFB.Infrastructure.Contexts;
using SFB.AMS.Backend.Repositories;
using SFB.Shared.Backend.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace SFB.AMS.Backend.Controllers
{
    [Route("api/AMS/[controller]/[action]")]
    public class AuthenticationController : BaseController<SGDContext, AuthenticationRepository>
    {
        public AuthenticationController(SGDContext context, IConfiguration configuration)
        {
            Repository = new AuthenticationRepository(context, configuration);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] MLogin login)
        {
            try
            {
                //var ipAddress = this.HttpContext.Connection.RemoteIpAddress;

                var result = await Repository.Login(login);

                return OkResult(result);
            }
            catch (ControllerException ex)
            {
                return ControlledExceptionResult(ex);
            }
            catch (Exception ex)
            {
                return InternalServerErrorResult(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                //var ipAddress = this.HttpContext.Connection.RemoteIpAddress;
                var user = User.FindFirst("NameUser")?.Value;

                var result = await Repository.RefreshToken(user);

                return OkResult(result);
            }
            catch (ControllerException ex)
            {
                return ControlledExceptionResult(ex);
            }
            catch (Exception ex)
            {
                return InternalServerErrorResult(ex);
            }
        }
    }
}
