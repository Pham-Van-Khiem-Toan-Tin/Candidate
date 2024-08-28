using Candidate.Form;
using Candidate.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Candidate.Controllers
{
    [Route("/api/candidate")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateRepository _candidateRepository;
        public CandidateController(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCandidate([FromForm] CandidateCreateForm candidateCreateForm, IFormFile? file)
        {
            var createResult = await _candidateRepository.CreateCandidate(candidateCreateForm, file);
            if (createResult)
            {
                return Ok(new { message= "Create candidate successfully" });
            } else return BadRequest("Create candidate fail");
        }
    }
}
