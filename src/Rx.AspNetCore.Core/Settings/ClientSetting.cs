using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx.AspNetCore.Core.Settings
{
    public class ClientSetting : BaseSetting
    {
        public ClientSetting(JObject jObject)
        {
            Configuration = jObject["configuration"];
        }
    }
}
