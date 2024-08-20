using Microsoft.AspNetCore.Identity;

namespace VotingSystem.Entities;

public class User : IdentityUser
{
    public bool Voted { get; set; } = false;
}