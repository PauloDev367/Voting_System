using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using VotingSystem.Data;
using VotingSystem.Dtos.Request;
using VotingSystem.Dtos.Responses;
using VotingSystem.Entities;
using VotingSystem.Repositories;
using VotingSystem.Services;

namespace VotingSystem.Hubs;

[Authorize]
public class AdminVotingHub : Hub
{
    private readonly AppDbContext _context;
    private readonly UserRepository _userRepository;
    private readonly VotingService _votingService;
    public AdminVotingHub(AppDbContext context, UserRepository userRepository, VotingService votingService)
    {
        _context = context;
        _userRepository = userRepository;
        _votingService = votingService;
    }

    public async Task GetVoteInformationAsync()
    {
        var response = await _votingService.GetSystemOverviewAsync();
        await SendGroupMessage(response, "LoadSystemData");
    }

    public async Task GetTotalVotesPerAgentAsync()
    {
        var total = await _votingService.GetTotalVotesPerAgentsAsync();
        await SendGroupMessage(total, "TotalPerAgent");
    }

    public async Task AddNewAgent(NewAgentRequest request)
    {
        var agent = await _votingService.AddNewAgentAsync(request.AgentName);
        var response = new VotesPerAgentResponse(agent);
        await Clients.All.SendAsync("NewAgentRegistered", response);
    }

    public async Task RemoveAgentAsync(string agentId)
    {
        await _votingService.RemoveAgentAsync(new Guid(agentId));
        await GetTotalVotesPerAgentAsync();
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

        await _votingService.AddConnectionIdToUserAsync(userId, connectionIdCurrent);
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