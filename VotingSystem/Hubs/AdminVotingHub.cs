using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using VotingSystem.Data;
using VotingSystem.Dtos.Responses;

namespace VotingSystem.Hubs;

[Authorize]
public class AdminVotingHub : Hub
{
    private readonly AppDbContext _context;

    public AdminVotingHub(AppDbContext context)
    {
        _context = context;
    }

    public async Task GetVoteInformationAsync()
    {
        var votes = await _context.Votes.ToListAsync();
        var totalVotes = votes.Count();
        var users = await _context.Users.ToListAsync();
        var totalUsers = users.Count();
        var totalUsersVotes = users.Select(u=>u.Voted).Count();
        var totalUsersOnline = users.Select(u=>u.IsOnline).Count();
        var response = new VoteInformationResponse
        {
            TotalVotes = totalVotes,
            TotalUsers = totalUsers,
            TotalUsersThatVoted = totalUsersVotes,
            TotalUsersOnline = totalUsersOnline,
            TotalUsersThatNotVoted = (totalVotes - totalUsersVotes)
        };

    }
    public async Task EndVoteAsync(){}
    public async Task RestartVoteAsync(){}
    public async Task ShowAwardAsync(){}
    
}