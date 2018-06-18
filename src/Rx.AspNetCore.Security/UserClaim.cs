using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rx.AspNetCore.Security
{
    public static class UserClaim
    {
        /// <summary>
        /// ClaimTypes.Email
        /// </summary>
        public static string Email
        {
            get
            {
                return GetClaimValue(ClaimTypes.Email);
            }
        }

        public static bool Anonymous
        {
            get
            {
                return GetClaimValue(ClaimTypes.Anonymous) != string.Empty;
            }
        }

        public static int TimeZone
        {
            get
            {
                var timeZone = GetClaimValue(ClaimTypes.Locality);
                return timeZone != string.Empty ? Convert.ToInt32(timeZone) : 0;
            }
        }

        public static int Country
        {
            get
            {
                var country = GetClaimValue(ClaimTypes.Country);
                return country != string.Empty ? Convert.ToInt32(country) : 0;
            }
        }


        public static string DbServer
        {
            get
            {
                return GetClaimValue(ClaimTypes.System);
            }
        }


        public static string CompanyName {
            get {
                return GetClaimValue(ClaimTypes.GivenName);
            }
        }
        public static int Company
        {
            get
            {
                var companyId = GetClaimValue(ClaimTypes.GroupSid);
                return companyId != string.Empty ? Convert.ToInt32(companyId) : 0;
            }
        }

        public static string Hash
        {
            get
            {
                return GetClaimValue(ClaimTypes.Hash);
            }
        }


        /// <summary>
        /// ClaimTypes.Uri
        /// </summary>
        public static string Uri
        {
            get
            {
                return GetClaimValue(ClaimTypes.Uri);
            }
        }

        /// <summary>
        /// ClaimTypes.Sid
        /// </summary>
        public static int UserId
        {
            get
            {
                var value = GetClaimValue(ClaimTypes.NameIdentifier);
                return value == string.Empty ? 0 : Convert.ToInt32(value);
            }
        }

        public static Guid UserGuid
        {
            get
            {
                var claimObject = ((System.Security.Claims.ClaimsPrincipal)(Thread.CurrentPrincipal)).Claims.Where(t => t.Type == ClaimTypes.NameIdentifier).SingleOrDefault();
                return (claimObject != null) ? new Guid(claimObject.Value) : Guid.Empty;
            }
        }

        /// <summary>
        /// ClaimTypes.NameIdentifier
        /// </summary>
        public static string UserName
        {
            get
            {
                return GetClaimValue(ClaimTypes.Name);
            }
        }

        public static string LanguageCode {
            get
            {
                return GetClaimValue(ClaimTypes.UserData);
            }
        }


        public static string Get(string name)
        {
            return GetClaimValue(name);
        }

        private static string GetClaimValue(string claim)
        {
            var claimObject = ((System.Security.Claims.ClaimsPrincipal)(Thread.CurrentPrincipal)).Claims.Where(t => t.Type == claim).SingleOrDefault();
            return (claimObject != null) ? claimObject.Value : string.Empty;
        }
    }
}
