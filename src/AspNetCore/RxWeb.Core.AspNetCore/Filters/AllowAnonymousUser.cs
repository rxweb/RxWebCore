using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RxWeb.Core.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace RxWeb.Core.AspNetCore.Filters
{
    public class AllowAnonymousUser : ActionFilterAttribute, IAllowAnonymous
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var tokenAuthorizer = context.HttpContext.RequestServices.GetService(typeof(ITokenAuthorizer)) as ITokenAuthorizer;
            var result = tokenAuthorizer.AnonymousUserValidateToken(context.HttpContext);
            if (result == null)
                context.Result = new StatusCodeResult(401);
        }
    }
}
