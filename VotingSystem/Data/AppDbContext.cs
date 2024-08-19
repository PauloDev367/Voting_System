using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VotingSystem.Entities;

namespace VotingSystem.Data;

public class AppDbContext : IdentityDbContext
{
    public DbSet<Vote> Votes { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}