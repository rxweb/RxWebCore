using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx.AspNetCore.Security.Jwt
{
    public interface ITokenValidator
    {
        bool Validate(HttpContext httpContext);
    }
}
