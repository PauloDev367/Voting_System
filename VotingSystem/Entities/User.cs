using Microsoft.AspNetCore.Identity;

namespace VotingSystem.Entities;

public class User : IdentityUser
{
    public bool Voted { get; set; } = false;
    public bool IsOnline { get; set; } = false;
    public List<HubId> HubIds { get; set; }
}