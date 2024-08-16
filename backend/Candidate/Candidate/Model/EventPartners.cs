namespace Candidate.Model
{
    public class EventPartners
    {
        public string EventId { get; set; }
        public Event Event { get; set; }
        public string PartnerId { get; set; }
        public Partner Partner { get; set; }
    }
}
