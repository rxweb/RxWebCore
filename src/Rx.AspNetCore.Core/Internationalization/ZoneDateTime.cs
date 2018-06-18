using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rx.AspNetCore.Core.Internationalization
{
    public class ZoneDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            
            if (value is DateTime )
            {
                DateTime dateObject = Convert.ToDateTime(value);
                if (dateObject > DateTime.MinValue) {
                    var userTimeZone = GetUserTimeZone();
                    if (!string.IsNullOrEmpty(userTimeZone))
                    {
                        var zoneProvider = DateTimeZoneProviders.Tzdb[userTimeZone];
                        var zoneDateTime = Instant.FromDateTimeUtc((DateTime)value)
                                      .InZone(zoneProvider)
                                      .ToDateTimeUnspecified();
                        writer.WriteValue(zoneDateTime);
                    }
                }
            }
            else
            {
                throw new Exception("Expected date object value.");
            }
        }

        private string GetUserTimeZone() {
            var claimObject = ((System.Security.Claims.ClaimsPrincipal)(Thread.CurrentPrincipal)).Claims.Where(t => t.Type == ClaimTypes.Locality).SingleOrDefault();
            return (claimObject != null) ? claimObject.Value : string.Empty;
        }
    }


}
