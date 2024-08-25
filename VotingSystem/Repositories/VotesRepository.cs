using Microsoft.EntityFrameworkCore;
using VotingSystem.Data;
using VotingSystem.Entities;

namespace VotingSystem.Repositories;

public class VotesRepository
{
    private readonly AppDbContext _context;

    public VotesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Vote>> GetAllVotesAsync()
    {
        return await _context.Votes.ToListAsync();
    }
}