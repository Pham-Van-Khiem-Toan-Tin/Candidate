using Candidate.Data;
using Candidate.Interface;
using Candidate.Model;
using Candidate.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Candidate.Repository
{
    public class ChannelRepository : IChannelRepository
    {
        private readonly ApplicationDBContext _context;

        public ChannelRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public Task<List<ChannelDTO>> GetAll()
        {
            return _context.Channels.Select(c => new ChannelDTO
            {
                Id = c.Id,
                Name = c.Name,
                Link = c.Link,
                Description = c.Description,
            }).ToListAsync();
        }
    }
}
