namespace Candidate.Model
{
    public class EventChannels
    {
        public string EventId { get; set; }
        public Event Event { get; set; }
        public string ChannelId { get; set; }
        public Channel Channel { get; set; }
    }
}
