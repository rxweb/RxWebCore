using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Rx.AspNetCore.Security.Formatters
{
    public class ApplicationInputFormatter : IInputFormatter
    {
        [OnSerialized]
        public bool CanRead(InputFormatterContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var contentType = context.HttpContext.Request.ContentType;
            if (contentType == null || contentType == "application/json")
                return true;
            return false;
        }

        public Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var request = context.HttpContext.Request; if (request.ContentLength == 0)
            {
                if (context.ModelType.GetTypeInfo().IsValueType)
                    return InputFormatterResult.SuccessAsync(Activator.CreateInstance(context.ModelType));
                else return InputFormatterResult.SuccessAsync(null);
            }

            using (var reader = new StreamReader(context.HttpContext.Request.Body))
            {
                var data = reader.ReadToEnd();
                if (!string.IsNullOrEmpty(data)) {
                    var model = JsonConvert.DeserializeObject(data, context.ModelType);
                    reader.Dispose();
                    reader.Close();
                    return InputFormatterResult.SuccessAsync(model);
                }
                return InputFormatterResult.SuccessAsync(null);
            }
        }
    }
}
