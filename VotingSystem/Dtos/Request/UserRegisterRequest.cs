using System.ComponentModel.DataAnnotations;

namespace VotingSystem.Dtos.Request;

public class UserRegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [StringLength(50)]
    public string Senha { get; set; }
    [Compare(nameof(Senha))]
    public string SenhaConfirmacao { get; set; }
}