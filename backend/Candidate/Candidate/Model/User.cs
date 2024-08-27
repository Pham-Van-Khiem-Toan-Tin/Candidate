using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Candidate.Model
{
    public class User : IdentityUser
    {

        [PersonalData]
        public string FullName {  get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
