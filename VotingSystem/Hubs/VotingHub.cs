using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using VotingSystem.Data;
using VotingSystem.Dtos.Request;
using VotingSystem.Entities;

namespace VotingSystem.Hubs;

[Authorize(Roles = "CLIENT")]
public class VotingHub : Hub
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly AppDbContext _context;

    public VotingHub(UserManager<IdentityUser> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }
    
    public async Task AddVoteAsync(AddVoteRequest request)
    {
        var email = Context.User?.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

        if (email != null)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var userVote = await _context.Votes.FirstOrDefaultAsync(vt => vt.UserId.Equals(user.Id));
                if (userVote == null)
                {
                    var vote = new Vote
                    {
                        UserId = user.Id,
                        OptionVoted = request.OptionVoted,
                    };
                    await _context.Votes.AddAsync(vote);
                    await _context.SaveChangesAsync();
                    await Clients.All.SendAsync("UpdateTotalVotes", _context.Votes.Count());
                }
            }
        }

    }
    public async Task GetVotesAsync(){}
    
}