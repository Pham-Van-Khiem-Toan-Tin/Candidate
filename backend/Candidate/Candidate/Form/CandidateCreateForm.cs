using Candidate.Model;

namespace Candidate.Form
{
    public class CandidateCreateForm
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string UniversityId { get; set; }
        public string Skills { get; set; }
        public string Major { get; set; }
        public string? Language { get; set; }
        public string Graduation { get; set; }
        public string GPA { get; set; }
        public DateTime ApplyDate { get; set; }
        public string WorkingTime { get; set; }
        public string Status { get; set; }
        public string? Note { get; set; }
        public string Positions { get; set; }
        public string EventId { get; set; }
        public string channelId { get; set; }
    }
}
