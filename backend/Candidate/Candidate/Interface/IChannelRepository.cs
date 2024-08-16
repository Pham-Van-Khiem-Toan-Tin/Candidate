using Candidate.Rsps;

namespace Candidate.Interface
{
    public interface IChannelRepository
    {
        Task<List<ChannelDTO>> GetAll();
    }
}
