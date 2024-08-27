using Candidate.DTOs;
using Candidate.Interface;
using Candidate.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Candidate.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public Task<List<UserDTO>> GetAllUsers()
        {
            return _userManager.Users
            .Include(e => e.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Select(u => new UserDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Status = u.Status,
                Address = u.Address,
                DateOfBirth = u.DateOfBirth,
                Roles = u.UserRoles.Select(u => u.Role.Name).FirstOrDefault(),
            }).ToListAsync();
            
        }
        public async Task<PagedResult<UserDTO>> Search(int pageNumber, int pageSize, string status, string name)
        {
            const int maxPageSize = 20;
            pageSize = (pageSize > maxPageSize) ? maxPageSize : pageSize;
            var query = _userManager.Users
            .Include(e => e.UserRoles).ThenInclude(ur => ur.Role).AsQueryable();
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(u => u.FullName.Contains(name));
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                bool isActive = status.Equals("Activated", StringComparison.OrdinalIgnoreCase);
                query = query.Where(u => u.Status == isActive);
            }
            var totalItems = await query.CountAsync();
            var users = await query.Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(u => new UserDTO
                                    {
                                        Id = u.Id,
                                        FullName = u.FullName,
                                        Email = u.Email,
                                        PhoneNumber = u.PhoneNumber,
                                        Status = u.Status,
                                        Address = u.Address,
                                        DateOfBirth = u.DateOfBirth,
                                        Roles = u.UserRoles.Select(u => u.Role.Name).FirstOrDefault(),
                                    }).ToListAsync();
            return new PagedResult<UserDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                Items = users
            };
        }
    }
}
