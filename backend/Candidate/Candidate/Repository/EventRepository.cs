using Candidate.Data;
using Candidate.DTOs;
using Candidate.Interface;
using Candidate.Model;
using Microsoft.EntityFrameworkCore;

namespace Candidate.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDBContext _context;
        public EventRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public Task<List<EventDTO>> GetAllEventsInProgess()
        {
            return _context.Events
                .Include(e => e.EventPartners)
                .ThenInclude(ep => ep.Partner)
                .Include(e => e.EventChannels)
                .ThenInclude(ec => ec.Channel)
                .Include(e => e.EventPositions)
                .ThenInclude(epo => epo.Position)
                .Where(e => e.Status == "In-progress").Select(e => new EventDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    Channels = e.EventChannels.Select(ec => new ChannelDTO
                    {
                        Id = ec.Channel.Id,
                        Name = ec.Channel.Name,
                        Description = ec.Channel.Description,
                        Link = ec.Channel.Link
                    }).ToList(),
                    Partners = e.EventPartners.Select(ep => new PartnerDTO
                    {
                        Id = ep.Partner.Id,
                        Name = ep.Partner.Name,
                        Address = ep.Partner.Address,
                    }).ToList(),
                    Positions = e.EventPositions.Select(epo => new PositionDTO
                    {
                        Id = epo.Position.Id,
                        Name= epo.Position.Name,
                    }).ToList(),
                }).ToListAsync();
        }
        public async Task<bool> CreateEvent(Event eventDes)
        {
            
            _context.Events.Add(eventDes);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        public Task<List<EventDTO>> GetAllEvents()
        {
            return _context.Events
                .Include(e => e.EventPartners)
                .ThenInclude(ep => ep.Partner)
                .Include(e => e.EventChannels)
                .ThenInclude(ec => ec.Channel).Select(e => new EventDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    Target = e.Target,
                    Participants = e.Participants,
                    Note = e.Note,
                    Status = e.Status,
                    Channels = e.EventChannels.Select(ec => new ChannelDTO
                    {
                        Id = ec.Channel.Id,
                        Name = ec.Channel.Name,
                        Description = ec.Channel.Description,
                        Link = ec.Channel.Link
                    }).ToList(),
                    Partners = e.EventPartners.Select(ep => new PartnerDTO
                    {
                        Id = ep.Partner.Id,
                        Name = ep.Partner.Name,
                        Address = ep.Partner.Address,
                    }).ToList(),
                }).ToListAsync();
        }
        public async Task<PagedResult<EventDTO>> Search(int pageNumber, int pageSize, DateTime? startDate, DateTime? endDate, string name)
        {
            var query = _context.Events
                .Include(e => e.EventPartners)
                .ThenInclude(ep => ep.Partner)
                .Include(e => e.EventChannels)
                .ThenInclude(ec => ec.Channel)
                .AsQueryable();
            if (startDate.HasValue)
            {
                query = query.Where(e => e.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(e => e.EndDate <= endDate.Value);
            }

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Name.Contains(name));
            }
            var totalItems = await query.CountAsync();

            var events = await query.Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(e => new EventDTO
                                    {
                                        Id = e.Id,
                                        Name = e.Name,
                                        StartDate = e.StartDate,
                                        EndDate = e.EndDate,
                                        Target = e.Target,
                                        Participants = e.Participants,
                                        Note = e.Note,
                                        Status = e.Status,
                                        Channels = e.EventChannels.Select(ec => new ChannelDTO
                                        {
                                            Id = ec.Channel.Id,
                                            Name = ec.Channel.Name,
                                            Description = ec.Channel.Description,
                                            Link = ec.Channel.Link
                                        }).ToList(),
                                        Partners = e.EventPartners.Select(ep => new PartnerDTO
                                        {
                                            Id = ep.Partner.Id,
                                            Name = ep.Partner.Name,
                                            Address = ep.Partner.Address,
                                        }).ToList(),
                                    }).ToListAsync();
            return new PagedResult<EventDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                Items = events
            };
        }
        public Task<EventDTO> GetEventById(string id)
        {
            return _context.Events
                .Include(e => e.EventPartners)
                .ThenInclude(ep => ep.Partner)
                .Include(e => e.EventChannels)
                .ThenInclude(ec => ec.Channel)
                .Where(e => e.Id == id)
                .Select(e => new EventDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    Target = e.Target,
                    Participants = e.Participants,
                    Note = e.Note,
                    Status = e.Status,
                    Channels = e.EventChannels.Select(ec => new ChannelDTO
                    {
                        Id = ec.Channel.Id,
                        Name = ec.Channel.Name,
                        Description = ec.Channel.Description,
                        Link = ec.Channel.Link
                    }).ToList(),
                    Partners = e.EventPartners.Select(ep => new PartnerDTO
                    {
                        Id = ep.Partner.Id,
                        Name = ep.Partner.Name,
                        Address = ep.Partner.Address,
                    }).ToList(),
                }).SingleOrDefaultAsync();
        }
        public async Task<bool> DeleteEvent(string id)
        {
            var eventToDelete = await _context.Events
                .Include(e => e.EventPartners)
                .Include(e => e.EventChannels)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (eventToDelete == null)
            {
                return false;
            }
            _context.EventPartners.RemoveRange(eventToDelete.EventPartners);
            _context.EventChannels.RemoveRange(eventToDelete.EventChannels);

            _context.Events.Remove(eventToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
