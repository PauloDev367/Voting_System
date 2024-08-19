using System.ComponentModel.DataAnnotations;

namespace VotingSystem.Dtos.Request;

public class UserLoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Senha { get; set; }
}