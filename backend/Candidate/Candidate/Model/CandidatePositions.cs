namespace Candidate.Model
{
    public class CandidatePositions
    {
        public string CandidateInfoId { get; set; }
        public string PositionId { get; set; }
        public CandidateInfo CandidateInfo { get; set; }
        public Position Position { get; set; }
    }
}
