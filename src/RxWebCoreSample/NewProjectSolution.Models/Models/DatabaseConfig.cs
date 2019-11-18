using System.Collections.Generic;

namespace NewProjectSolution.Models
{
    public class DatabaseConfig
    {
		public MultiTenantConfig MultiTenant { get; set; } = new MultiTenantConfig();

        public Dictionary<string,string> ConnectionString { get; set; }

        public Dictionary<string,int> ConnectionResiliency { get; set; }

        public int CommandTimeout { get; set; }
    }
}

