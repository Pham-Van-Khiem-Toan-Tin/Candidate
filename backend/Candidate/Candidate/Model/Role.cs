using Microsoft.AspNetCore.Identity;

namespace Candidate.Model
{
    public class Role : IdentityRole
    {
        public virtual ICollection<UserRoles> UserRoles { get; set; }
    }
}
