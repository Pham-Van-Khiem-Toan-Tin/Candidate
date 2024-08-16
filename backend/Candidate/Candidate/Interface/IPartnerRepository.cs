using Candidate.Rsps;

namespace Candidate.Interface
{
    public interface IPartnerRepository
    {
        Task<List<PartnerDTO>> GetAll();
    }
}
