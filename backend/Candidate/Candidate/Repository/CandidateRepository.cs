using Candidate.Data;
using Candidate.DTOs;
using Candidate.Form;
using Candidate.Interface;
using Candidate.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Candidate.Repository
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CandidateRepository(ApplicationDBContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor) 
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> CreateCandidate(CandidateCreateForm candidateCreateForm, IFormFile file)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var userId = user?.Id;
            var university = await _context.Partners
                .FirstOrDefaultAsync(u => u.Id == candidateCreateForm.UniversityId);
            if (university == null)
                return false;
            var eventApply = await _context.Events
                .Include(e => e.EventChannels)
                .ThenInclude(ec => ec.Channel)
                .Include(e => e.EventPositions)
                .ThenInclude(ep => ep.Position)
                .FirstOrDefaultAsync(e => e.Id == candidateCreateForm.EventId);
            if (eventApply == null)
            {
                return false;
            }
            
            bool channelExist = eventApply.EventChannels.Any(ec => ec.ChannelId == candidateCreateForm.channelId);
            var postions = JsonConvert.DeserializeObject<List<string>>(candidateCreateForm.Positions);
            bool positionExist = postions.All(positionId => eventApply.EventPositions.Any(ep => ep.PositionId == positionId));
            if (!channelExist || !positionExist)
            {
                return false;
            }
            string filePath = null;
            if (file != null && file.Length > 0) 
            {
                var fileName = Path.GetFileName(file.FileName);
                filePath = Path.Combine("Uploads", fileName);
                if ( !Directory.Exists("Uploads"))
                {
                    Directory.CreateDirectory("Uploads");
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            List<string> skills = JsonConvert.DeserializeObject<List<string>>(candidateCreateForm.Skills);

            List<string> languages = new List<string>();
            if (!string.IsNullOrEmpty(candidateCreateForm.Language))
            {
                languages = JsonConvert.DeserializeObject<List<string>>(candidateCreateForm.Language);
            }
            CandidateInfo newCandidate = new CandidateInfo
            {
                Id = candidateCreateForm.Id,
                FullName = candidateCreateForm.FullName,
                Email = candidateCreateForm.Email,
                DateOfBirth = candidateCreateForm.DateOfBirth,
                PhoneNumber = candidateCreateForm.PhoneNumber,
                Address = candidateCreateForm.Address,
                Gender = candidateCreateForm.Gender,
                UniversityId = candidateCreateForm.UniversityId,
                Skills = String.Join(", ", skills),
                Major = candidateCreateForm.Major,
                Language = languages.Count > 0 ? String.Join(", ", languages) : "N/A",
                Graduation = int.Parse(candidateCreateForm.Graduation),
                LinkCV = filePath ?? "N/A",
                GPA = float.Parse(candidateCreateForm.GPA),
                WorkingTime = candidateCreateForm.WorkingTime,
                Status = candidateCreateForm.Status,
                Note =  candidateCreateForm.Note ?? "N/A",
                CreatedAt = DateTime.Now,
                UserId = userId,
                Applications = new List<Application>
                    {
                        new Application
                        {
                            CandidateId = candidateCreateForm.Id,
                            EventId = candidateCreateForm.EventId,
                            ChannelId = candidateCreateForm.channelId,
                            ApplyDate = candidateCreateForm.ApplyDate,
                            Status = true,
                            CandidatePositions = postions.Select(positionId => new CandidatePositions
                            {
                                CandidateInfoId = candidateCreateForm.Id,
                                EventId = candidateCreateForm.EventId,
                                PositionId = positionId
                            }).ToList(),
                        }
                    }

            };
            _context.CandidateInfos.Add(newCandidate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CandidateDTO>> GetAllCandidate()
        {
            var query = _context.CandidateInfos
                .Include(c => c.Applications)
                    .ThenInclude(a => a.Event)
                .Include(c => c.Applications)
                    .ThenInclude(a => a.Channel)
                .Include(c => c.CandidatePositions)
                    .ThenInclude(cp => cp.Position)
                 .Select(c => new CandidateDTO
                 {
                     Id = c.Id,
                     FullName = c.FullName,
                     Email = c.Email,
                     DateOfBirth = c.DateOfBirth,
                     PhoneNumber = c.PhoneNumber,
                     Address = c.Address,
                     Gender = c.Gender,
                     Skills = c.Skills,
                     Major = c.Major,
                     Language = c.Language,
                     Graduation = c.Graduation,
                     LinkCV = c.LinkCV,
                     GPA = c.GPA,
                     WorkingTime = c.WorkingTime,
                     Status = c.Status,
                     Note = c.Note,
                     CreatedAt = c.CreatedAt,
                     EventInfo = c.Applications.Where(a => a.Status == true).OrderBy(a => a.ApplyDate).Select(a => new EventDTO
                     {
                         Id = a.Event.Id,
                         Name = a.Event.Name,
                         //Positions = c.Applications.Where(a => a.Status == true).OrderBy(a => a.ApplyDate).ToList(),
                         //Channels = c.Applications.Where(a => a.Status == true).OrderBy(a => a.ApplyDate).Select(a => new ChannelDTO
                         //{
                         //    Id = a.Channel.Id,
                         //    Name = a.Channel.Name
                         //}).ToList()
                     }).FirstOrDefault(),
                     University = new PartnerDTO
                     {
                         Id = c.Partner.Id,
                         Name = c.Partner.Name,
                     },
                     Positions = c.CandidatePositions.Select(cp => new PositionDTO
                     {
                         Id = cp.Position.Id,
                         Name = cp.Position.Name,
                     }).ToList(),
                     Channel = c.Applications.Where(a => a.Status == true).OrderBy(a => a.ApplyDate).Select(a => new ChannelDTO
                     {
                         Id = a.Channel.Id,
                         Name = a.Channel.Name
                     }).FirstOrDefault()

                 });
            return await query.ToListAsync();

        }
    }
}
