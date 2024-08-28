using System.ComponentModel.DataAnnotations;

namespace VotingSystem.Dtos.Request;

public class AddVoteRequest
{
    [Required]
    public Guid OptionVoted { get; set; }
}