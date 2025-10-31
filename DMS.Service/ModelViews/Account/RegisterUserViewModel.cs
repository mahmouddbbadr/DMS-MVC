
using System.ComponentModel.DataAnnotations;
namespace DMS.Service.ModelViews.Account
{
    public class RegisterUserViewModel
    {
        [MinLength(2), MaxLength(15)]
        public string FirstName { get; set; }
        [MinLength(2), MaxLength(15)]
        public string LastName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Invalid email format.")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Weak password.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
