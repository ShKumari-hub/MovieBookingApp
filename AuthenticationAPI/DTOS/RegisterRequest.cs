using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.DTOS
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required,EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string LoginId { get; set; } = string.Empty;

        [Required,DataType(DataType.PhoneNumber)]
        public string ContactNumber { get; set; } = string.Empty;

        [Required,DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required,Compare(nameof(Password),ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
        
    }
}
