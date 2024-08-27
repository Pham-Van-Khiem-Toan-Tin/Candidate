using Microsoft.AspNetCore.Identity;

namespace Candidate.Model
{
    public class UserRoles : IdentityUserRole<string>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }  
    }
}
