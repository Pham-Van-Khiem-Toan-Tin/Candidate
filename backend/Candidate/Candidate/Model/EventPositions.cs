namespace Candidate.Model
{
    public class EventPositions
    {
        public string EventId { get; set; }
        public Event Event { get; set; }
        public string PositionId { get; set; }
        public Position Position { get; set; }
    }
}
