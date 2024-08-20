using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VotingSystem.Entities;

namespace VotingSystem.Data.Configurations;

public class VoteConfig : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.HasOne(x => x.Agent)
            .WithMany(a => a.Votes)
            .HasForeignKey(x => x.AgentId);
    }
}