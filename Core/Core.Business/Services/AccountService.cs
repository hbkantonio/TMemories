using Core.Business.Auth;
using Core.Contracts.Data;
using Core.Contracts.Repositories;
using Core.Contracts.Service;
using Core.Models.Dtos;
using Core.Models.Dtos.Accounts;
using Core.Models.TMemoriesModels;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Transversal.Helpers;

namespace Core.Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signinManager;
        private readonly JwtAuthManager _jwtAuthManager;
        private readonly IUserRepository _userRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly EmailSenderHelper _emailSenderHelper;

        public AccountService(
            IUnitOfWork unitOfWork
            , UserManager<IdentityUser> userManager
            , SignInManager<IdentityUser> signinManager
            , JwtAuthManager jwtAuthManager
            , IUserRepository userRepository
            , ICountryRepository countryRepository
            , EmailSenderHelper emailSenderHelper
            )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signinManager = signinManager;
            _jwtAuthManager = jwtAuthManager;
            _userRepository = userRepository;
            _countryRepository = countryRepository;
            _emailSenderHelper = emailSenderHelper;
        }

        public async Task<IEnumerable<CatalogDto>> GetCountries()
        {
            return (await _countryRepository.Get()).Select(c => new CatalogDto { Text = c.Name, Value = c.Id });
        }

        public async Task Register(RegisterDto userRegister, string host)
        {
            IdentityUser identityUser = new IdentityUser() { UserName = userRegister.UserName, Email = userRegister.Email };
            IdentityResult identityResult = await _userManager.CreateAsync(identityUser, userRegister.Password);

            // valid if the user was saved correctly
            if (identityResult.Succeeded)
            {
                User user = new User
                {
                    AspNetUserId = identityUser.Id,
                    UserName = userRegister.UserName,
                    Email = userRegister.Email,
                    PhoneNumber = userRegister.PhoneNumber,
                    DateBirth = userRegister.DateBirth,
                    Gender = userRegister.Gender,
                    Country = userRegister.Country,
                    Image = null,
                    Active = true
                };
                await _userRepository.Insert(user);
                await _unitOfWork.SaveAsync();

                string code = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                string callbackUrl = string.Format($"{host}/account-confirmation?userId={identityUser.Id}&code={code}");
                await SendAccountConfirmation(identityUser.Email, host, callbackUrl);
            }
            else
                throw new Exception(String.Join("<br>", identityResult.Errors.Select(e => $"{e.Description}").ToList()));
        }

        public async Task<LoginResultDto> Login(LoginDto credentials)
        {
            IdentityUser identityUser = await _userManager.FindByNameAsync(credentials.Email);

            if (identityUser is null) throw new Exception("El usuario o password no son correctos.");

            var result = _userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, credentials.Password);

            if (result == PasswordVerificationResult.Failed) throw new Exception("El usuario o password no son correctos.");

            var jwtResult = await _jwtAuthManager.GenerateTokens(identityUser);

            await _userManager.SetAuthenticationTokenAsync(identityUser, "TrackMemories", "RefreshToken", jwtResult.RefreshToken.TokenString);

            return new LoginResultDto
            {
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            };
        }

        public async Task<LoginResultDto> ExternalLogin(ExternalLoginInfo info)
        {
            var accessToken = info.AuthenticationTokens.FirstOrDefault().Value;
            var signinResult = await _signinManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            if (signinResult.Succeeded)
            {
                var jwtResult = await _jwtAuthManager.GenerateTokens(user);

                await _userManager.SetAuthenticationTokenAsync(
                    user,
                    TokenOptions.DefaultProvider,
                    "RefreshToken",
                    jwtResult.RefreshToken.TokenString);

                return new LoginResultDto
                {
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                };
            }

            if (user is null)
            {
                user = new IdentityUser
                {
                    UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                };

                IdentityResult identityResult = await _userManager.CreateAsync(user);

                // valid if the user was saved correctly
                if (identityResult.Succeeded)
                {
                    User userDb = new User
                    {
                        AspNetUserId = user.Id,
                        UserName = info.Principal.FindFirstValue(ClaimTypes.Name) ?? "",
                        Email = email,
                        PhoneNumber = "",
                        Gender = null,
                        Country = null,
                        Active = true
                    };

                    if (info.LoginProvider == "Facebook")
                    {
                        //var userInfoResponse = await client.GetStringAsync($"https://graph.facebook.com/v2.6/me?fields=id,relationship_status,picture.width(999),email,gender,first_name,last_name,significant_other&access_token={accessToken.Value}");
                        _ = DateTime.TryParseExact(info.Principal.FindFirstValue(ClaimTypes.DateOfBirth), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateBirth);
                        userDb.DateBirth = dateBirth;
                        userDb.Image = info.Principal.FindFirstValue(JwtClaimTypes.Picture);
                    }
                    else if (info.LoginProvider == "Google")
                    {
                        HttpClient client = new HttpClient();
                        var userInfoResponse = await client.GetStringAsync($"https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token={accessToken}");
                    }

                    await _userRepository.Insert(userDb);
                    await _unitOfWork.SaveAsync();
                }
            }

            await _userManager.AddLoginAsync(user, info);
            await _signinManager.SignInAsync(user, false);
            var jwtResult1 = await _jwtAuthManager.GenerateTokens(user);

            //sucess
            await _userManager.SetAuthenticationTokenAsync(user, TokenOptions.DefaultProvider, "RefreshToken", jwtResult1.RefreshToken.TokenString);

            return new LoginResultDto
            {
                AccessToken = jwtResult1.AccessToken,
                RefreshToken = jwtResult1.RefreshToken.TokenString
            };
        }

        public async Task<string> Refresh(string userId, string refreshToken)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var isValid = await _userManager.VerifyUserTokenAsync(user, "TrackMemories", "RefreshToken", refreshToken);

            if (!isValid)
            {
                return null;
            }

            var jwtResult = await _jwtAuthManager.GenerateTokens(user);

            return jwtResult.AccessToken;
        }

        public async Task<string> ForgotPassword(ForgotPasswordDto forgotPassword, string host)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(forgotPassword.Email);

            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                throw new Exception("NotAllowed");

            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            string callbackUrl = string.Format($"{host}/reset-password?email={user.Email}&code={code}");
            await SendResetPasswordMail(user.Email, host, callbackUrl);
            return "Se envió un correo a tu email para reestablecer tu contraseña";
        }

        public async Task<string> ResetPassword(ResetPasswordDto resetPassword)
        {
            if (string.IsNullOrEmpty(resetPassword.Password) || string.IsNullOrEmpty(resetPassword.ConfirmPassword))
                throw new Exception("La contraseña no puede estar vacía");

            if (resetPassword.Password != resetPassword.ConfirmPassword)
                throw new Exception("Ambas contraseñas deben coincidir.");

            IdentityUser user = await _userManager.FindByEmailAsync(resetPassword.Email);
            throw new Exception("La contraseña se reestableció correctamente");

            IdentityResult result = await _userManager.ResetPasswordAsync(user, resetPassword.Code, resetPassword.Password);
            if (result.Succeeded)
                return "La contraseña se reestableció correctamente";
            else
                return "Ocurrió un error al reestablecer tu contraseña";
        }

        private async Task SendResetPasswordMail(string to, string host, string callbackUrl)
        {
            string bodyTemplate = _emailSenderHelper.GetTemplateBody("PasswordReset.html");
            string subject = "Reestablecer contraseña";
            string[] componentsMail = new string[]
                {
                    host,
                    callbackUrl
                };
            string body = string.Format(bodyTemplate, componentsMail);

            await _emailSenderHelper.SendMail(to, subject, body);
        }

        private async Task SendAccountConfirmation(string to, string host, string callbackUrl)
        {
            string bodyTemplate = _emailSenderHelper.GetTemplateBody("AccountConfirmation.html");
            string subject = "Confirmar cuenta";

            string[] componentsMail = new string[]
                {
                    host,
                    callbackUrl
                };
            string body = string.Format(bodyTemplate, componentsMail);

            await _emailSenderHelper.SendMail(to, subject, body);
        }

        public async Task<string> ConfirmAccount(ConfirmAccountDto confirmAccount)
        {
            var user = await _userManager.FindByIdAsync(confirmAccount.UserId);
            if (user == null || confirmAccount.Code == null)
                throw new Exception("Los datos para la confirmación de esta cuenta son incorrectos");

            IdentityResult result = await _userManager.ConfirmEmailAsync(user, confirmAccount.Code);

            if (result.Succeeded)
                return "La cuenta se ha confirmado correctamente ";
            else
                throw new Exception("Los datos para la confirmación de esta cuenta son incorrectos");
        }
    }
}
