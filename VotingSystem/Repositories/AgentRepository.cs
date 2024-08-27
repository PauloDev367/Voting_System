using Microsoft.EntityFrameworkCore;
using VotingSystem.Data;
using VotingSystem.Entities;

namespace VotingSystem.Repositories;

public class AgentRepository
{
    private readonly AppDbContext _context;

    public AgentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Agent>> GetAllAsync()
    {
        return await _context.Agents
            .Include(a => a.Votes)
            .ToListAsync();
    }

    public async Task<Agent> SaveAgentAsync(Agent agent)
    {
        await _context.Agents.AddAsync(agent);
        await _context.SaveChangesAsync();
        return agent;
    }

    public async Task<Agent?> GetOneByIdAsync(Guid id)
    {
        return await _context.Agents.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }


    public async Task RemoveAsync(Agent agent)
    {
        _context.Agents.Remove(agent);
        await _context.SaveChangesAsync();
    }

    public async Task SetAllTotalVotesToZero()
    {
        var votes = await _context.Agents
            .Include(a => a.Votes)
            .ToListAsync();

        foreach (var vote in votes)
        {
            vote.Votes.Clear();
        }
        await _context.SaveChangesAsync();
    }
}