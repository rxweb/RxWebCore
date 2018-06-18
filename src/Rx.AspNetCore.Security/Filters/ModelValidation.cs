using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx.AspNetCore.Security.Filters
{
    public class ModelValidation : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            var errors = new JObject();
            if (!modelState.IsValid)
            {
                foreach (var key in modelState.Keys)
                {
                    var state = modelState[key];
                    if (state.Errors.Any())
                    {
                        errors[key] = state.Errors.First().ErrorMessage;
                    }
                }
                ErrorResponse(errors, context);
                return;
            }
            if (context.HttpContext.Request.Method.ToUpper() == "POST" || context.HttpContext.Request.Method.ToUpper() == "PUT")
            {
                var controller = (ControllerBase)context.Controller;
                if (context.ActionArguments.Count > 0)
                {
                    foreach (var entity in context.ActionArguments) {
                        if (entity.Value is IDefaultData)
                        {
                            ((IDefaultData)entity.Value).ApplyDefault();
                        }
                    }
                    
                }
            }
            base.OnActionExecuting(context);
        }
        private void ErrorResponse(JObject errors, ActionExecutingContext context)
        {
            context.Result = new ContentResult()
            {
                Content = errors.ToString()
            };
            context.HttpContext.Response.StatusCode = 400;
        }
    }
}
