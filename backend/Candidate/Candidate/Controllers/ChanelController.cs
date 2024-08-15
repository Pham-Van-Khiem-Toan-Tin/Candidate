using Candidate.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Candidate.Controllers
{
    [Route("api/chanel")]
    public class ChanelController : Controller
    {
        private readonly IChanelRepository _chanelRepository;
        public ChanelController(IChanelRepository chanelRepository) 
        {
            _chanelRepository = chanelRepository;
        }
        [Authorize(Roles = "Admin, Leader")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var chanels = await _chanelRepository.GetAll();
            return Ok(chanels);
        }
    }
}
