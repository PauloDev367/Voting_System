using System.ComponentModel.DataAnnotations;

namespace VotingSystem.Dtos.Request;

public class AddVoteRequest
{
    [Required]
    public string OptionVoted { get; set; }
}