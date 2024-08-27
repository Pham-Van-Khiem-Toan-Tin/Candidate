using Candidate.DTOs;

namespace Candidate.Interface
{
    public interface IPositionRepository
    {
        Task<List<PositionDTO>> GetAllPosition();
    }
}
