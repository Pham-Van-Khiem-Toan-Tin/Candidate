namespace Candidate.Model
{
    public class Position
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Department {  get; set; }
        public string Location { get; set;}
        public ICollection<EventPositions> EventPositions { get; set; }
        public ICollection<CandidatePositions> CandidatePositions { get; set; }
    }
}
