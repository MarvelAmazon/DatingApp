using System.ComponentModel.DataAnnotations;

namespace API;

public class RegisterDto
{
    [Required]
    public string Username { get; set; } // Declare the Username property
    [Required]
    [StringLength(8, MinimumLength = 4)]
    public string Password { get; set; } // Declare the Password property
}
