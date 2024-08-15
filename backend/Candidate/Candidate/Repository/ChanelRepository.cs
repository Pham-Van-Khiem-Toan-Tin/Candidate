using Candidate.Data;
using Candidate.Interface;
using Candidate.Model;
using Microsoft.EntityFrameworkCore;

namespace Candidate.Repository
{
    public class ChanelRepository : IChanelRepository
    {
        private readonly ApplicationDBContext _context;

        public ChanelRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public Task<List<Chanel>> GetAll()
        {
            return _context.Chanels.ToListAsync();
        }
    }
}
