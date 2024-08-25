using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using VotingSystem.Data;
using VotingSystem.Dtos.Responses;
using VotingSystem.Repositories;

namespace VotingSystem.Hubs;

[Authorize]
public class AdminVotingHub : Hub
{
    private readonly AppDbContext _context;
    private readonly UserRepository _userRepository;

    public AdminVotingHub(AppDbContext context, UserRepository userRepository)
    {
        _context = context;
        _userRepository = userRepository;
    }

    public async Task GetVoteInformationAsync()
    {
        var votes = await _context.Votes.ToListAsync();
        var totalVotes = votes.Count();
        var users = await _context.Users.ToListAsync();
        var totalUsers = users.Count();
        var totalUsersVotes = users.Where(u => u.Voted != false).Count();
        var totalUsersOnline = users.Where(u => u.IsOnline == true).ToList().Count();
        var count = (totalVotes - totalUsersVotes);
        var response = new VoteInformationResponse
        {
            TotalVotes = totalVotes,
            TotalUsers = totalUsers,
            TotalUsersThatVoted = totalUsersVotes < 0 ? 0 : totalUsersVotes,
            TotalUsersOnline = totalUsersOnline,
            TotalUsersThatNotVoted = count < 0 ? 0 : count,
        };
        await SendGroupMessage(response, "LoadSystemData");
    }

    public async Task EndVoteAsync()
    {
    }

    public async Task RestartVoteAsync()
    {
    }

    public async Task ShowAwardAsync()
    {
    }

    public async Task AddConnectionIdToUser()
    {
        var connectionIdCurrent = Context.ConnectionId;
        var userClaims = Context.User;
        var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        var user = await _userRepository.GetOneByIdAsync(userId);
        if(user != null)
            await _userRepository.AddConnectionIdToUserAsync(user, connectionIdCurrent);
    }

    private async Task SendGroupMessage(object message, string method)
    {
        var users = await _userRepository.GetAdminsAsync();

        foreach (var user in users)
        {
            foreach (var connId in user.HubIds)
            {
                await Groups.AddToGroupAsync(connId.ClientHubId, "AdminEnable");
            }
        }

        await Clients.Group("AdminEnable").SendAsync(method, message);
    }
}