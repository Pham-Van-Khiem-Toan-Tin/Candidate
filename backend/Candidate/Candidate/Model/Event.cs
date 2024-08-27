using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Candidate.Model
{
    public class Event
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<EventPartners> EventPartners { get; set; }
        public ICollection<EventChannels> EventChannels { get; set; }
        public ICollection<EventPositions> EventPositions { get; set; }
        public ICollection<Application> Applications { get; set; }
        public string Target { get; set; }
        public int Participants { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
    }
}
