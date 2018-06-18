using System;

namespace Rx.AspNetCore.Cache.Models
{
    public class TagCache
    {
        public Type Controller { get; set; }

        public string Etag { get; set; }
    }
}
