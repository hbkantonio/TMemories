using Core.Models.Dtos.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Auth
{
    public class JwtAuthManager
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettingsDto _jwtSettings;

        public JwtAuthManager(UserManager<IdentityUser> userManager, IOptions<JwtSettingsDto> jwtTokenOptions)
        {
            _userManager = userManager;
            _jwtSettings = jwtTokenOptions.Value;
        }

        public async Task<JwtAuthResultDto> GenerateTokens(IdentityUser identityUser)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim("Id", identityUser.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddSeconds(_jwtSettings.ExpiryTimeInMinutes),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Audience = _jwtSettings.Audience,
                    Issuer = _jwtSettings.Issuer
                };

                var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
                var accessTokenString = tokenHandler.WriteToken(jwtToken);
                var refreshTokenstring = await _userManager.GenerateUserTokenAsync(identityUser, TokenOptions.DefaultProvider, "RefreshToken");

                var refreshTokenModel = new RefreshTokenDto
                {
                    UserName = identityUser.Email,
                    TokenString = refreshTokenstring,
                    ExpireAt = DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpiration)
                };

                return new JwtAuthResultDto
                {
                    AccessToken = accessTokenString,
                    RefreshToken = refreshTokenModel
                };
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }
    }
}
