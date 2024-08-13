using System.ComponentModel.DataAnnotations;

namespace Candidate.Dtos
{
    public class EditUserDTO
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
