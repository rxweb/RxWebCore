using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx.AspNetCore.Core.Settings
{
    public abstract class BaseSetting
    {
        public JToken Configuration { get; set; }
        public virtual T Get<T>(string jsonNode)
        {
            var keys = jsonNode.Split('.');
            dynamic token = null;
            var count = 0;
            foreach (var key in keys)
            {
                if (count == 0)
                    token = Configuration.Root[key];
                else
                    token = token[key];
                count++;
            }
            return (T)token;
        }
    }
}
