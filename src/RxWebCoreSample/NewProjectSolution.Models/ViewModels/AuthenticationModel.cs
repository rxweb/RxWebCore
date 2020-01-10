
using RxWeb.Core.Annotations;
using RxWeb.Core.Sanitizers;
using System;

namespace NewProjectSolution.Models.ViewModels
{
    public partial class AuthenticationModel {
        public bool ValidateUserName(AuthenticationModel o) {
            var t = o;
            //o.UserName = "ajay";
            return false;
        }
    }
    
    [ModelValidation("validationTitle")]
    public partial class AuthenticationModel
    {
        [OnAction("POST",RxWeb.Core.Sanitizers.Enums.ActionValueType.DateTimeUtc)]
        public DateTime CreatedDate { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
        
    }
}

