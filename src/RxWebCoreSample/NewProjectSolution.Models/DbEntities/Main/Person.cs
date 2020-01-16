using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using RxWeb.Core.Annotations;
using RxWeb.Core.Data.Annotations;
using RxWeb.Core.Sanitizers;
using NewProjectSolution.Models.Enums.Main;
using NewProjectSolution.BoundedContext.SqlContext;
namespace NewProjectSolution.Models.Main
{
    [Table("Persons",Schema="dbo")]
    [RecordLog]
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
		#endregion PersonName Annotations

        public string PersonName { get; set; }


        public Nullable<bool> IsActive { get; set; }


        public Person()
        {
        }
	}
}