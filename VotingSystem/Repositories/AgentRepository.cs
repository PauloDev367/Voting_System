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
        return await _context.Agents.ToListAsync();
    }
}