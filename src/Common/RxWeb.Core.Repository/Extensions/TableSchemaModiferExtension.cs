using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace RxWeb.Core.Data
{
    public static class TableSchemaModiferExtension
    {
        public static ModelBuilder AddSchemaBasedTenant(this ModelBuilder modelBuilder, string schema)
        {
            var entityTypes = modelBuilder.Model.GetEntityTypes().Select(t => t.ClrType).ToList();
            var entities = entityTypes.Where(t => t.GetCustomAttributes(typeof(TableAttribute), true).Count() > 0);
            var enumerator = entities.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var tableAttribute = enumerator.Current.GetCustomAttributes(typeof(TableAttribute), true).SingleOrDefault() as TableAttribute;
                if (tableAttribute != null) {
                    modelBuilder.Entity(enumerator.Current).ToTable(tableAttribute.Name, schema);
                }
            }
            return modelBuilder;
        }
    }
}
