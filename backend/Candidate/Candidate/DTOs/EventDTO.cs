
namespace Candidate.Rsps
{
    public class EventDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Target { get; set; }
        public int Participants { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public List<PartnerDTO> Partners { get; set; }
        public List<ChannelDTO> Channels { get; set; }
    }
}
