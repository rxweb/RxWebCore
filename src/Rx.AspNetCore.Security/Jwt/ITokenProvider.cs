using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rx.AspNetCore.Security.Jwt
{
    public interface ITokenProvider
    {
        Dictionary<byte[],string> WriteToken(IEnumerable<Claim> claims, string issuer, string audience);

        bool ValidateToken(byte[] securityKey, string jsonWebToken);

        void LogOut();
    }
}
