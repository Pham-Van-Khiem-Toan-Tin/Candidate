using Candidate.Model;

namespace Candidate.Interface
{
    public interface IUserRepository
    {
        Task<List<User>> getAllUsers();
    }
}
