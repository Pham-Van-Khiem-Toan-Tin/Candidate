using Candidate.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Candidate.Controllers
{
    [Route("api/partner")]
    [ApiController]
    public class PartnerController : ControllerBase
    {
        private readonly IPartnerRepository _partnerRepository;
        public PartnerController(IPartnerRepository partnerRepository)
        {
            _partnerRepository = partnerRepository;
        }
        [Authorize(Roles = "Admin, Leader")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllPartner() 
        {
            var partners = await _partnerRepository.GetAll();
            return Ok(partners);
        }
    }
}
