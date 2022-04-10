using Core.Models.Dtos;
using Core.Models.Dtos.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Core.Contracts.Service
{
    public interface IAccountService
    {
        Task<IEnumerable<CatalogDto>> GetCountries();
        Task Register(RegisterDto userRegister, string host);
        Task<LoginResultDto> Login(LoginDto credentials);
        Task<LoginResultDto> ExternalLogin(ExternalLoginInfo info);
        Task<string> ForgotPassword(ForgotPasswordDto forgotPassword, string host);
        Task<string> ResetPassword(ResetPasswordDto resetPassword);
        Task<string> ConfirmAccount(ConfirmAccountDto confirmAccount);
    }
}
