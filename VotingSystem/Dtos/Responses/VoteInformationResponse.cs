namespace VotingSystem.Dtos.Responses;

public class VoteInformationResponse
{
    public int TotalVotes { get; set; }
    public int TotalUsers { get; set; }
    public int TotalUsersThatVoted { get; set; }
    public int TotalUsersThatNotVoted { get; set; }
    public int TotalUsersOnline { get; set; }
    public bool VoteStatus { get; set; }
}