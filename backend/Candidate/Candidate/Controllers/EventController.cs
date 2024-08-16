using Candidate.Dtos;
using Candidate.Interface;
using Candidate.Model;
using Candidate.Rsps;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Candidate.Controllers
{
    [Route("api/event")]
    [ApiController]
    public class EventController : Controller
    {
        private readonly IEventRepository _eventRepository;
        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> Search(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null, 
            [FromQuery] string name = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var results = await _eventRepository.GetAllEvent(pageNumber, pageSize, startDate, endDate, name);
            return Ok(results);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<ActionResult> CreateEvent([FromBody] CreateEventForm createEventDTO)
        {
            if (createEventDTO == null)
            {
                return BadRequest();
            }
            var newEvent = new Event
            {
                Id = createEventDTO.Id,
                Name = createEventDTO.Name,
                StartDate = createEventDTO.StartDate,
                EndDate = createEventDTO.EndDate,
                Target = createEventDTO.Target ?? string.Empty,
                Note = createEventDTO.Note ?? string.Empty,
                Participants = createEventDTO.Participants,
                Status = "New",
                EventPartners = createEventDTO.PartnerIds.Select(id => new EventPartners { PartnerId = id }).ToList(),
                EventChannels = createEventDTO.ChannelIds.Select(id => new EventChannels { ChannelId = id }).ToList(),
            };
            var result = await _eventRepository.CreateEvent(newEvent);
            if (result)
            {
                return Ok(new { Message = "Event created successfully!" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating event.");
            }

        }
    }
}
