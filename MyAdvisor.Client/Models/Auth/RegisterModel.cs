using System.ComponentModel.DataAnnotations;

namespace MyAdvisor.Client.Models.Auth;

public class RegisterModel
{
    [Required]
    public string FirstName { get; set; } = "";

    [Required]
    public string LastName { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; } = "";

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string Confirm { get; set; } = "";
}
