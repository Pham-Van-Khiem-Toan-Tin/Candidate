using Candidate.DTOs;

namespace Candidate.Interface
{
    public interface IChannelRepository
    {
        Task<List<ChannelDTO>> GetAll();
    }
}
