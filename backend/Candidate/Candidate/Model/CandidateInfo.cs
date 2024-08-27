using System.ComponentModel.DataAnnotations;

namespace Candidate.Model
{
    public class CandidateInfo
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
        public string Language {  get; set; }
        public int Graduation { get; set; }
        public string LinkCV { get; set; }
        public float GPA { get; set; }
        public DateTime ApplyDate { get; set; }
        public string WorkingTime { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public Partner Partner { get; set; }
        public ICollection<Application> Applications { get; set; }
        public ICollection<CandidatePositions> CandidatePositions { get; set; }
    }
}
