namespace VotingSystem.Entities;

public class Agent
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Vote> Votes { get; set; }
}