using Candidate.Data;
using Candidate.DTOs;
using Candidate.Interface;
using Microsoft.EntityFrameworkCore;

namespace Candidate.Repository
{
    public class PositionRepository : IPositionRepository
    {
        private readonly ApplicationDBContext _context;
        public PositionRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public Task<List<PositionDTO>> GetAllPosition()
        {
            return _context.Positions.Select(p => new PositionDTO
            {
                Id = p.Id,
                Name = p.Name,
                Location = p.Location,
                Department = p.Department,
            }).ToListAsync();
        }
    }
}
