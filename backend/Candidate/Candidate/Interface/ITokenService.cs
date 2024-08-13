using Candidate.Model;

namespace Candidate.Interface
{
    public interface ITokenService
    {
        string GenerateToken(User user, List<String> roles);
    }
}
