namespace Candidate.Model
{
    public class Partner
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<EventPartners> EventPartners { get; set; }
    }
}
