using System.ComponentModel.DataAnnotations;

namespace NattfrostBackend.DTOs
{
    public class CreateSubscribeRequest
    {
        [Required (ErrorMessage = "Email is required.")]
        [EmailAddress (ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;
        
        [Required (ErrorMessage = "City is required.")]
        public string City { get; set; } = string.Empty;
    }
}
