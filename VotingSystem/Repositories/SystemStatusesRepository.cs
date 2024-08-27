using System;
using Microsoft.EntityFrameworkCore;
using VotingSystem.Data;

namespace VotingSystem.Repositories;

public class SystemStatusesRepository
{
    private readonly AppDbContext _context;

    public SystemStatusesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> VoteIsActiveAsync()
    {
        var status = await _context.SystemStatuses.FirstAsync();
        return status.VoteIsActive;
    }
    public async Task ChangeVoteStatusAsync(bool newStatus)
    {
        var status = await _context.SystemStatuses.FirstAsync();
        status.VoteIsActive = newStatus;
        _context.SystemStatuses.Update(status);
        await _context.SaveChangesAsync();

    }
}
