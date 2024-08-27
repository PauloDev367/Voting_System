using Microsoft.EntityFrameworkCore;
using VotingSystem.Data;
using VotingSystem.Dtos.Responses;
using VotingSystem.Entities;
using VotingSystem.Repositories;

namespace VotingSystem.Services;

public class VotingService
{
    private readonly AppDbContext _context;
    private readonly UserRepository _userRepository;
    private readonly VotesRepository _voteRepository;
    private readonly AgentRepository _agentRepository;

    public VotingService(AppDbContext context, UserRepository userRepository, VotesRepository voteRepository,
        AgentRepository agentRepository)
    {
        _context = context;
        _userRepository = userRepository;
        _voteRepository = voteRepository;
        _agentRepository = agentRepository;
    }

    public async Task<VoteInformationResponse> GetSystemOverviewAsync()
    {
        var votes = await _context.Votes.ToListAsync();
        var totalVotes = votes.Count();
        var users = await _context.Users.ToListAsync();
        var totalUsers = users.Count();
        var totalUsersVotes = users.Where(u => u.Voted != false).Count();
        var totalUsersOnline = users.Where(u => u.IsOnline == true).ToList().Count();
        var count = (totalVotes - totalUsersVotes);
        var response = new VoteInformationResponse()
        {
            TotalVotes = totalVotes,
            TotalUsers = totalUsers,
            TotalUsersThatVoted = totalUsersVotes < 0 ? 0 : totalUsersVotes,
            TotalUsersOnline = totalUsersOnline,
            TotalUsersThatNotVoted = count < 0 ? 0 : count,
        };
        return response;
    }

    public async Task AddConnectionIdToUserAsync(string userId, string connectionId)
    {
        var user = await _userRepository.GetOneByIdAsync(userId);
        if (user != null)
            await _userRepository.AddConnectionIdToUserAsync(user, connectionId);
    }

    public async Task<List<VotesPerAgentResponse>> GetTotalVotesPerAgentsAsync()
    {
        var votes = await _agentRepository.GetAllAsync();
        return votes.Select(v => new VotesPerAgentResponse(v)).ToList();
    }

    public async Task<Agent> AddNewAgentAsync(string agentName)
    {
        var agent = new Agent
        {
            Name = agentName
        };

        await _agentRepository.SaveAgentAsync(agent);
        return agent;
    }

    public async Task RemoveAgentAsync(Guid id)
    {
        var agent = await _agentRepository.GetOneByIdAsync(id);
        if (agent == null)
            throw new Exception("Agent not found");

        await _agentRepository.RemoveAsync(agent);

    }
}