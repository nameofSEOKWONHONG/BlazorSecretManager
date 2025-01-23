using System.ComponentModel.DataAnnotations;

namespace BlazorSecretManager.Services.Auth.Requests;

public class RegisterRequest
{
    [Required]
    public string Email { get; set; }
    [Required]
    [StringLength(8,  ErrorMessage = "8자 이상 입력해야 합니다.")]
    public string Password { get; set; }
    [Required]
    [StringLength(8,  ErrorMessage = "8자 이상 입력해야 합니다.")]
    public string ConfirmPassword { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    
    /// <summary>
    /// staff...
    /// </summary>
    [Required]
    public string RoleName { get; set; }
    
    public static List<string> Types = new List<string> { "admin", "staff", "guest" };
}