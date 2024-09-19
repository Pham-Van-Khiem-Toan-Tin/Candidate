namespace Candidate.Model
{
    public class Application
    {
        public string CandidateId { get; set; }
        public string EventId { get; set; }
        public string ChannelId { get; set; }
        public DateTime ApplyDate { get; set; }
        public bool Status { get; set; }
        public CandidateInfo CandidateInfo { get; set; }
        public Event Event { get; set; }
        public Channel Channel { get; set; }
        public ICollection<CandidatePositions> CandidatePositions { get; set; }
    }
}
