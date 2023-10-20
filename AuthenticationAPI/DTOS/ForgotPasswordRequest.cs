using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.DTOS
{
    public class ForgotPasswordRequest
    {
        [Required]
        public string LoginId { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required, Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
