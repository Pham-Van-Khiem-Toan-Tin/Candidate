using Candidate.Model;
using Candidate.Rsps;

namespace Candidate.Interface
{
    public interface IEventRepository
    {
        Task<PagedResult<EventDTO>> GetAllEvent(int pageNumber, int pageSize, DateTime? startDate, DateTime? endDate, string name);
        Task<bool> CreateEvent(Event eventDes);
    }
}
