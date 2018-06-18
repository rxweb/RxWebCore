using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rx.AspNet.EntityFramework.Attributes
{
    public class SequenceNameAttribute: Attribute
    {
        public SequenceNameAttribute(string name, string schema)
        {
            Name = name;
            Schema = schema;
        }

        public string Name { get; private set; }
        public string Schema { get; set; }
    }
}
