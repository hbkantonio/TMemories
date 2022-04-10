using Core.Contracts.Service;
using Core.Models.Dtos;
using Core.Models.Dtos.Accounts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Presentation.WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signinManager;
        private readonly IAccountService _accountService;

        public AccountController(
            UserManager<IdentityUser> userManager
            , IAccountService accountService
            , SignInManager<IdentityUser> signInManager)
        {
            _accountService = accountService;
            _userManager = userManager;
            _signinManager = signInManager;
        }

        [HttpGet]
        [Route("Countries")]
        public async Task<IActionResult> GetCountries()
        {
            ResponseDto<IEnumerable<CatalogDto>> response = new ResponseDto<IEnumerable<CatalogDto>>();
            try
            {
                response.Data = await _accountService.GetCountries();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = ex.Message;
                return this.StatusCode(ex.InnerException is null ? 200 : 400, response);
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto userRegister)
        {
            ResponseDto<string> response = new ResponseDto<string>();
            try
            {
                await _accountService.Register(userRegister, Request.Headers["origin"]);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = ex.InnerException is null ? ex.Message : "Ocurrió un error al regístrate";
                return this.StatusCode(ex.InnerException is null ? 200 : 400, response);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto credentials)
        {
            ResponseDto<LoginResultDto> response = new ResponseDto<LoginResultDto>();
            try
            {
                response.Data = await _accountService.Login(credentials);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                response.Message = ex.InnerException is null ? ex.Message : "Ocurrió un error al iniciar sesión";
                return this.StatusCode(ex.InnerException is null ? 200 : 400, response);
            }
        }

        [Route("ExternalLogin/{provider}")]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signinManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [Route("ExternalLoginCallback")]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            ExternalLoginInfo info = await _signinManager.GetExternalLoginInfoAsync();

            var result = await _accountService.ExternalLogin(info);

            var options = new CookieOptions()
            {
                //Needed so that domain.com can access  the cookie set by api.domain.com
                Domain = Request.Headers["origin"],
                Expires = DateTime.UtcNow.AddMinutes(5)
            };

            Response.Cookies.Append(
                "tmt",
                JsonConvert.SerializeObject(result, new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    },
                    Formatting = Formatting.Indented
                }), options);

            return Redirect("/");
        }

        [HttpPost]
        [Route("ConfirmAccount")]
        public async Task<IActionResult> ConfirmAccount(ConfirmAccountDto confirmAccount)
        {
            ResponseDto<string> response = new ResponseDto<string>();

            try
            {
                response.Data = await _accountService.ConfirmAccount(confirmAccount);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                return this.StatusCode(ex.InnerException is null ? 200 : 400, response);
            }
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPassword)
        {
            ResponseDto<string> response = new ResponseDto<string>();
            try
            {
                response.Data = await _accountService.ForgotPassword(forgotPassword, Request.Headers["origin"]);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                return this.StatusCode(ex.InnerException is null ? 200 : 400, response);
            }
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPassword)
        {
            ResponseDto<string> response = new ResponseDto<string>();
            try
            {
                response.Data = await _accountService.ResetPassword(resetPassword);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccessful = false;
                return this.StatusCode(ex.InnerException is null ? 200 : 400, response);
            }
        }
    }
}
