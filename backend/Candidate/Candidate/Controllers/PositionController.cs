using Candidate.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Candidate.Controllers
{
    [Route("api/position")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IPositionRepository _positionRepository;
        public PositionController(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        [Authorize(Roles = "Admin, Leader")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPosition()
        {
            var positions = await _positionRepository.GetAllPosition();
            return Ok(positions);
        }
    }
}
