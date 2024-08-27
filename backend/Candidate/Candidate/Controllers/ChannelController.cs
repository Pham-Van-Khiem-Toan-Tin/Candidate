using Candidate.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Candidate.Controllers
{
    [Route("api/channel")]
    [ApiController]
    public class ChannelController : ControllerBase
    {
        private readonly IChannelRepository _channelRepository;
        public ChannelController(IChannelRepository channelRepository) 
        {
            _channelRepository = channelRepository;
        }
        [Authorize(Roles = "Admin, Leader")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var chanels = await _channelRepository.GetAll();
            return Ok(chanels);
        }
    }
}
