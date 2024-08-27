using Candidate.DTOs;
using Candidate.Model;

namespace Candidate.Interface
{
    public interface IUserRepository
    {
        Task<PagedResult<UserDTO>> Search(int pageNumber, int pageSize, string status, string name);
        Task<List<UserDTO>> GetAllUsers();
    }
}
