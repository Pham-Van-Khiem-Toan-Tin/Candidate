namespace Candidate.Dtos
{
    public class CreateEventForm
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Target { get; set; }
        public int Participants { get; set; }
        public string Note { get; set; }
        public List<String> PartnerIds { get; set; }
        public List<String> ChannelIds { get; set; }
        public List<String> Positions { get; set; }
    }
}
