using Candidate.DTOs;

namespace Candidate.Interface
{
    public interface IPartnerRepository
    {
        Task<List<PartnerDTO>> GetAll();
    }
}
