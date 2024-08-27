using Candidate.Data;
using Candidate.Interface;

namespace Candidate.Repository
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly ApplicationDBContext _context;
        public CandidateRepository(ApplicationDBContext context) 
        {
            _context = context;
        }
        public bool CreateCandidate()
        {
            
            return true;
        }
    }
}
