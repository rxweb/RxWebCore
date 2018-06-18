using System;

namespace Rx.AspNetCore.EntityFramework.Attributes
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
