using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Rx.AspNetCore.Core.Settings;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rx.AspNetCore.Security.Jwt
{
    public abstract class TokenProvider 
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public ServerSetting ServerSetting { get; set; }
        public TokenProvider(IHttpContextAccessor context, ServerSetting serverSetting)
        {
            HttpContextAccessor = context;
            ServerSetting = serverSetting;
        }

        public virtual bool ValidateToken(byte[] securityKey,string jsonWebToken)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(securityKey);
                var t = new TokenValidationParameters
                {
                    IssuerSigningKey = symmetricSecurityKey,
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
                SecurityToken sToken;
                var principal = new JwtSecurityTokenHandler().ValidateToken(jsonWebToken, t, out sToken);
                Thread.CurrentPrincipal = principal;
                return true;
        }

        public virtual KeyValuePair<byte[],string> WriteToken(IEnumerable<Claim> claims, string issuer, string audience,DateTime expires)
        {
            var securityKey = GetSymmetricKey();
            var symmetricSecurityKey = new SymmetricSecurityKey(securityKey);
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
              issuer: issuer,
              audience: audience,
              claims: claims,
              expires: expires,
              signingCredentials: credentials
              );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            var keyObject = new KeyValuePair<byte[], string>(securityKey, jwtToken.ToString());
            return keyObject;
        }


        private byte[] GetSymmetricKey()
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                byte[] secretKeyBytes = new Byte[32];
                provider.GetBytes(secretKeyBytes);
                return secretKeyBytes;
                //return Convert.ToBase64String(secretKeyBytes);
            }
        }
    }
}
