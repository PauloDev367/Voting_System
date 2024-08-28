using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
public class VotingHub : Hub
{
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _context;
    private readonly UserRepository _userRepository;
    private readonly VotingService _votingService;
    private readonly SystemStatusesRepository _systemStatusesRepository;
    public VotingHub(AppDbContext context, UserRepository userRepository, VotingService votingService, SystemStatusesRepository systemStatusesRepository, UserManager<User> userManager)
    {
        _context = context;
        _userRepository = userRepository;
        _votingService = votingService;
        _systemStatusesRepository = systemStatusesRepository;
        _userManager = userManager;
    }
    [Authorize(Roles = "ADMIN")]
    public async Task GetVoteInformationAsync()
    {
        var response = await _votingService.GetSystemOverviewAsync();
        await SendGroupMessage(response, "LoadSystemData");
    }
    [Authorize(Roles = "ADMIN")]
    public async Task AddNewAgent(NewAgentRequest request)
    {
        var agent = await _votingService.AddNewAgentAsync(request.AgentName);
        var response = new VotesPerAgentResponse(agent);
        await Clients.All.SendAsync("NewAgentRegistered", response);
    }
    [Authorize(Roles = "ADMIN")]
    public async Task RemoveAgentAsync(string agentId)
    {
        await _votingService.RemoveAgentAsync(new Guid(agentId));
        await GetTotalVotesPerAgentAsync();
    }
    [Authorize(Roles = "ADMIN")]
    public async Task OpenVoteAsync()
    {
        await _systemStatusesRepository.ChangeVoteStatusAsync(true);
        await Clients.All.SendAsync("VoteStatusChanged", true);
    }
    [Authorize(Roles = "ADMIN")]
    public async Task StopVoteAsync()
    {
        await _systemStatusesRepository.ChangeVoteStatusAsync(false);
        await Clients.All.SendAsync("VoteStatusChanged", false);
    }
    [Authorize(Roles = "ADMIN")]
    public async Task RestartVoteAsync()
    {
        await _votingService.RestartVoteAsync();
        await GetTotalVotesPerAgentAsync();
    }
    [Authorize(Roles = "ADMIN")]
    public async Task ShowWinnerAsync()
    {
        var winner = await _votingService.ShowVoteWinnerAsync();
        if (winner != null)
            await Clients.All.SendAsync("WinnerSelected", new VotesPerAgentResponse(winner));
    }

    [Authorize(Roles = "CLIENT")]
    public async Task AddVoteAsync(AddVoteRequest request)
    {
        var email = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
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
                        AgentId = request.OptionVoted,
                        OptionVoted = request.OptionVoted.ToString(),
                    };
                    await _context.Votes.AddAsync(vote);
                    await _context.SaveChangesAsync();
                    await GetTotalVotesPerAgentAsync();
                }
            }
        }

    }

    [Authorize(Roles = "CLIENT")]
    public async Task GetVoteInfosToClientAsync()
    {
        var data = await _votingService.GetClientInfo();
        await SendGroupOfClientsMessage(data, "LoadClientVoteInfo");

    }

    public async Task AddConnectionIdToUser()
    {
        var connectionIdCurrent = Context.ConnectionId;
        var userClaims = Context.User;
        var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        await _votingService.AddConnectionIdToUserAsync(userId, connectionIdCurrent);
    }
    public async Task Logout()
    {
        var email = Context.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (email != null)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                await _userRepository.RemoveUserConnectionIdAsync(user);
                await Clients.Caller.SendAsync("LogoutUser", "Usuário desconectado");
            }
        }
    }
    public async Task GetTotalVotesPerAgentAsync()
    {
        var total = await _votingService.GetTotalVotesPerAgentsAsync();
        await Clients.All.SendAsync("TotalPerAgent", total);
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
    private async Task SendGroupOfClientsMessage(object message, string method)
    {
        var users = await _userRepository.GetClientsAsync();

        foreach (var user in users)
        {
            foreach (var connId in user.HubIds)
            {
                await Groups.AddToGroupAsync(connId.ClientHubId, "ClientEnable");
            }
        }

        await Clients.Group("ClientEnable").SendAsync(method, message);
    }

}