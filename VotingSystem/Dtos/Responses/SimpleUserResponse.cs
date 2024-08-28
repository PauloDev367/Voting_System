using System;
using Microsoft.AspNetCore.Identity;
using VotingSystem.Entities;

namespace VotingSystem.Dtos.Responses;

public class SimpleUserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public List<string> Role { get; set; }

    public SimpleUserResponse()
    {
        Role = new List<string>();
    }

    public SimpleUserResponse(User user, UserManager<User> userManager)
    {
        Role = new List<string>();
        Id = new Guid(user.Id);
        Email = user.Email;
        var roles = userManager.GetRolesAsync(user).Result;
        Role = roles.ToList();
    }
}
