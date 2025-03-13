using System.ComponentModel.DataAnnotations;

namespace UserRoles.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Old Password is Required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Old Password is Required")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [Compare("ConfirmNewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm New Password is Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        public string ConfirmNewPassword { get; set; }
    }
}
