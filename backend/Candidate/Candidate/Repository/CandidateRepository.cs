using Candidate.Data;
using Candidate.Form;
using Candidate.Interface;
using Candidate.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Candidate.Repository
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly ApplicationDBContext _context;
        public CandidateRepository(ApplicationDBContext context) 
        {
            _context = context;
        }
        public async Task<bool> CreateCandidate(CandidateCreateForm candidateCreateForm, IFormFile file)
        {
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
                ApplyDate = candidateCreateForm.ApplyDate,
                WorkingTime = candidateCreateForm.WorkingTime,
                Status = candidateCreateForm.Status,
                Note =  candidateCreateForm.Note ?? "N/A",
                Applications = new List<Application>
                    {
                        new Application
                        {
                            CandidateId = candidateCreateForm.Id,
                            EventId = candidateCreateForm.EventId,
                            ChannelId = candidateCreateForm.channelId,
                            CandidateInfoPositions = postions.Select(positionId => new CandidatePositions
                            {
                                CandidateInfoId = candidateCreateForm.Id,
                                PositionId = positionId
                            }).ToList(),
                        }
                    }

            };
            _context.CandidateInfos.Add(newCandidate);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
