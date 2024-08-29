using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VotingSystem.Data;
using VotingSystem.Entities;

namespace VotingSystem.Repositories;

public class UserRepository
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserRepository(AppDbContext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<IList<User>> GetAdminsAsync()
    {
        var usersInRole = await _userManager.GetUsersInRoleAsync("ADMIN");

        var usersWithHubIds = usersInRole
            .Select(user => _userManager.Users
                .Include(u => u.HubIds)
                .FirstOrDefault(u => u.Id == user.Id))
            .ToList();

        return usersWithHubIds;
    }
    public async Task<IList<User>> GetClientsAsync()
    {
        var usersInRole = await _userManager.GetUsersInRoleAsync("CLIENT");

        var usersWithHubIds = usersInRole
            .Select(user => _userManager.Users
                .Include(u => u.HubIds)
                .FirstOrDefault(u => u.Id == user.Id))
            .ToList();

        return usersWithHubIds;
    }
    public async Task<User?> GetOneByIdAsync(string userId)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task AddConnectionIdToUserAsync(User user, string connectionId)
    {
        var hub = new HubId
        {
            UserId = new Guid(user.Id),
            ClientHubId = connectionId
        };
        user.IsOnline = true;
        user.HubIds.Add(hub);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveUserConnectionIdAsync(User user)
    {
        user.HubIds.RemoveAll(hb => hb.ClientHubId == user.Id);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    public async Task LogoutUser(User user)
    {
        user.IsOnline = false;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
    public async Task SetAllUserVoteAsFalseAsync()
    {
        var users = await _context.Users.ToListAsync();
        foreach (var user in users)
        {
            user.Voted = false;
        }

        _context.Users.UpdateRange(users);
        await _context.SaveChangesAsync();
    }
}