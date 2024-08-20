namespace VotingSystem.Entities;

public class Vote
{
    public Guid Id { get; set; }
    public string OptionVoted { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public Guid AgentId { get; set; }
    public Agent Agent { get; set; }
}