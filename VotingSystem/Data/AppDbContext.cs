using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using VotingSystem.Data.Configurations;
using VotingSystem.Entities;

namespace VotingSystem.Data;

public class AppDbContext : IdentityDbContext
{
    public DbSet<Vote> Votes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Agent> Agents { get; set; }
    public DbSet<HubId> HubIds { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new VoteConfig());
    }
}