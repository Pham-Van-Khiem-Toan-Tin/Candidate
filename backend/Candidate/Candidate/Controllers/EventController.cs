using Candidate.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Candidate.Controllers
{
    [Route("api/event")]
    [ApiController]
    public class EventController : Controller
    {
        private readonly IChanelRepository _chanelRepository;
        public EventController() { }
    }
}
