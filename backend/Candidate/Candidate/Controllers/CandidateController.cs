using Candidate.Form;
using Candidate.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> Search()
        {
            var candidates = await _candidateRepository.GetAllCandidate();
            return Ok(candidates);
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
