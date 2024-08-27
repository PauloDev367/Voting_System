using System;

namespace VotingSystem.Entities;

public class SystemStatus
{
    public Guid Id { get; set; }
    public bool VoteIsActive { get; set; } = false;
}
