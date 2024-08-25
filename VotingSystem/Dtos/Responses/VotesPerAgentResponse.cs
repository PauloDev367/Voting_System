using VotingSystem.Entities;

namespace VotingSystem.Dtos.Responses;

public class VotesPerAgentResponse
{
    public Guid AgentId { get; set; }
    public int TotalVotes { get; set; }
    public string AgentName { get; set; }

    public VotesPerAgentResponse()
    {
    }

    public VotesPerAgentResponse(Agent agent)
    {
        this.TotalVotes = agent.Votes.Count;
        this.AgentName = agent.Name;
        this.AgentId = agent.Id;
    }
}