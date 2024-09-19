namespace Candidate.Model
{
    public class CandidatePositions
    {
        public string CandidateInfoId { get; set; }
        public string EventId { get; set; }
        public string PositionId { get; set; }
        public Application Application { get; set; }
        public Position Position { get; set; }
    }
}
