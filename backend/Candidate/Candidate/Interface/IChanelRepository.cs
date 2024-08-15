using Candidate.Model;

namespace Candidate.Interface
{
    public interface IChanelRepository
    {
        Task<List<Chanel>> GetAll();
    }
}
