using System;

namespace Rx.AspNetCore.EntityFramework
{
    internal class InstanceRepository
    {
        public Type Entity { get; set; }

        public object Repository  { get; set; }
        
    }
}
