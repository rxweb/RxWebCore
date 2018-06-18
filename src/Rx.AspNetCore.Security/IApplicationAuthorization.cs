using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx.AspNetCore.Security
{
    public interface IApplicationAuthorization
    {
        Dictionary<string,object> Validate(HttpContext httpContext);
    }
}
