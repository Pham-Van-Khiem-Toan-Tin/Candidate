using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Candidate.Controllers
{
    [Route("/api/candidate")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCandidate()
        {
            return Ok(new { message= "Create candidate successfully" });
        }
    }
}
