namespace NewProjectSolution.Models
{
    public class MultiTenantConfig
    {
        public bool SchemaBasedMultiTenant { get; set; }

        public string TenantColumnName { get; set; }
    }
}

