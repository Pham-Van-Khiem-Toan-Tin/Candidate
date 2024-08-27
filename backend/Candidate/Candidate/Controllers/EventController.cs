using Candidate.Dtos;
using Candidate.Interface;
using Candidate.Model;
using Candidate.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace Candidate.Controllers
{
    [Route("api/event")]
    [ApiController]
    public class EventController : ControllerBase
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
            var results = await _eventRepository.Search(pageNumber, pageSize, startDate, endDate, name);
            return Ok(results);
        }
        [Authorize(Roles = "Admin, Leader")]
        [HttpGet("in-progress")]
        public async Task<IActionResult> GetAllEventInProgess()
        {
            var result = await _eventRepository.GetAllEventsInProgess();
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetEventDetail(string id)
        {
            var result = await _eventRepository.GetEventById(id);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("export")]
        public async Task<IActionResult> ExportEvent()
        {
            var events = await _eventRepository.GetAllEvents();
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Events");
            worksheet.Cells[1, 1].Value = "Name";
            worksheet.Cells[1, 2].Value = "Chanel";
            worksheet.Cells[1, 3].Value = "Start Date";
            worksheet.Cells[1, 4].Value = "End Date";
            worksheet.Cells[1, 5].Value = "Partner";
            worksheet.Cells[1, 6].Value = "Target";
            worksheet.Cells[1, 7].Value = "Total participant";
            worksheet.Cells[1, 8].Value = "Note";
            worksheet.Cells[1, 9].Value = "Status";
            for (int i = 0; i < events.Count; i++)
            {
                worksheet.Cells[i + 2, 1].Value = events[i].Name;
                worksheet.Cells[i + 2, 2].Value = String.Join(", ", events[i].Channels.Select(c => c.Name).ToList());
                if (events[i].StartDate != null)
                {
                    worksheet.Cells[i + 2, 3].Value = events[i].StartDate; // Excel tự động nhận diện DateTime
                    worksheet.Cells[i + 2, 3].Style.Numberformat.Format = "yyyy-MM-dd"; // Định dạng ngày tháng trong Excel
                }
                else
                {
                    worksheet.Cells[i + 2, 3].Value = ""; // Hoặc giá trị mặc định nếu không có ngày sinh
                }
                if (events[i].EndDate != null)
                {
                    worksheet.Cells[i + 2, 4].Value = events[i].EndDate; // Excel tự động nhận diện DateTime
                    worksheet.Cells[i + 2, 4].Style.Numberformat.Format = "yyyy-MM-dd"; // Định dạng ngày tháng trong Excel
                }
                else
                {
                    worksheet.Cells[i + 2, 4].Value = ""; // Hoặc giá trị mặc định nếu không có ngày sinh
                }
                worksheet.Cells[i + 2, 5].Value = String.Join(", ", events[i].Partners.Select(p => p.Id).ToList());
                worksheet.Cells[i + 2, 6].Value = events[i].Target;
                worksheet.Cells[i + 2, 7].Value = events[i].Participants;
                worksheet.Cells[i + 2, 8].Value = events[i].Note;
                worksheet.Cells[i + 2, 9].Value = events[i].Status;
            }
            worksheet.Cells.AutoFitColumns();

            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            var fileName = "Users.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventForm createEventDTO)
        {
            if (createEventDTO == null)
            {
                return BadRequest();
            }
            var newEvent = new Event
            {
                Id = StringUtil.FormatEventName(createEventDTO.Name),
                Name = createEventDTO.Name,
                StartDate = createEventDTO.StartDate,
                EndDate = createEventDTO.EndDate,
                Target = createEventDTO.Target ?? string.Empty,
                Note = createEventDTO.Note ?? string.Empty,
                Participants = createEventDTO.Participants,
                Status = "New",
                EventPartners = createEventDTO.PartnerIds.Select(id => new EventPartners { PartnerId = id }).ToList(),
                EventChannels = createEventDTO.ChannelIds.Select(id => new EventChannels { ChannelId = id }).ToList(),
                EventPositions = createEventDTO.Positions.Select(id => new EventPositions { PositionId = id }).ToList(),
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
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEvent(string id)
        {
            var result = await _eventRepository.DeleteEvent(id);
            if (result)
            {
                return Ok(new { message= "Event deleted successfully." });
            }
            else
            {
                return NotFound("Event not found.");
            }
        }
    }
}
