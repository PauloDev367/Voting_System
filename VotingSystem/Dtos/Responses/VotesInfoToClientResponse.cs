using System;
using VotingSystem.Entities;

namespace VotingSystem.Dtos.Responses;

public class VotesInfoToClientResponse
{
    public int TotalVotes { get; set; }
    public bool VoteIsOpen { get; set; }

    public VotesInfoToClientResponse()
    {
    }
}
