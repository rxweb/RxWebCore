using System;

namespace Rx.AspNetCore.EntityFramework.Attributes
{
    public class RelationshipTableAttribue : Attribute
    {
        public RelationshipTableAttribue(string name, string schema)
        {
            Name = name;
            Schema = schema;
        }

        public string Name { get; private set; }
        public string Schema { get; set; }
    }
}
