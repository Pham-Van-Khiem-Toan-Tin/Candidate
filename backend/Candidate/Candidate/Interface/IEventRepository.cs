using Candidate.Model;
using Candidate.DTOs;

namespace Candidate.Interface
{
    public interface IEventRepository
    {
        Task<List<EventDTO>> GetAllEvents();
        Task<PagedResult<EventDTO>> Search(int pageNumber, int pageSize, DateTime? startDate, DateTime? endDate, string name);
        Task<EventDTO> GetEventById(string id);
        Task<bool> CreateEvent(Event eventDes);
        Task<bool> DeleteEvent(string id);
        Task<List<EventDTO>> GetAllEventsInProgess();
    }
}
