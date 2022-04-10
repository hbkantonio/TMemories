using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Dtos.Auth
{
    public class JwtAuthResultDto
    {
        public string AccessToken { get; set; }
        public RefreshTokenDto RefreshToken { get; set; }
    }
}
