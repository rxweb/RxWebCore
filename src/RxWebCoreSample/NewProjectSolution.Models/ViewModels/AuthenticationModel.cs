
using RxWeb.Core.Annotations;

namespace NewProjectSolution.Models.ViewModels
{
    public partial class AuthenticationModel {
        public bool ValidateUserName(AuthenticationModel o) {
            var t = o;
            o.UserName = "ajay";
            return false;
        }
    }
    public enum Status { 
    Active=1,
    InActive
    }
    public partial class AuthenticationModel
    {

        [Required(conditionalExpressionName:nameof(AuthenticationModel.ValidateUserName))]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public Status StatusId { get; set; }
    }
}

