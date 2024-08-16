using Candidate.Data;
using Candidate.Interface;
using Candidate.Rsps;
using Microsoft.EntityFrameworkCore;

namespace Candidate.Repository
{
    public class PartnerRepository : IPartnerRepository
    {
        private readonly ApplicationDBContext _context;
        public PartnerRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public Task<List<PartnerDTO>> GetAll()
        {
            return _context.Partners.Select(p => new PartnerDTO
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.Address,
            }).ToListAsync();
        }
    }
}
