using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using RxWeb.Core.Annotations;
using RxWeb.Core.Data.Annotations;
using RxWeb.Core.Sanitizers;
using NewProjectSolution.Models.Enums.Main;
using NewProjectSolution.BoundedContext.SqlContext;
using RxWeb.Core.Annotations.Models;

namespace NewProjectSolution.Models.Main
{
    [Table("Persons", Schema = "dbo")]
    public partial class Person
    {
        #region PersonId Annotations

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [System.ComponentModel.DataAnnotations.Key]
        #endregion PersonId Annotations

        public int PersonId { get; set; }

        #region PersonName Annotations

        [Required]
        [MaxLength(50)]
        [Unique(connection: typeof(IMainDatabaseFacade), uniqueQueryMethod:nameof(Person.UniqueQueryMethod))]
        #endregion PersonName Annotations

        public string PersonName { get; set; }

        private List<UniqueQuery> UniqueQueryMethod()
        {
            var uniqueQueries = new List<UniqueQuery> {
            new UniqueQuery{
            ColumnName = "IsActive",
            QueryOperator= RxWeb.Core.Annotations.Enums.SqlQueryOperator.NotEqual,
            Value = false
            }
            };
            return uniqueQueries;
        }
        public Person()
        {
        }
    }
}