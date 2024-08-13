using Candidate.Data;
using Candidate.Interface;
using Candidate.Model;
using Microsoft.EntityFrameworkCore;

namespace Candidate.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;

        public UserRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public Task<List<User>> getAllUsers()
        {
            return _context.User.ToListAsync();
        }
    }
}
