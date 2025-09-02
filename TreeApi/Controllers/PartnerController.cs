using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TreeApi.Services;

namespace TreeApi.Controllers
{
    [ApiController]
    [Route("api.user.partner")]
    [SwaggerTag("Represents partner API")]
    public class PartnerController : ControllerBase
    {
        private readonly IPartnerService _partnerService;

        public PartnerController(IPartnerService partnerService)
        {
            _partnerService = partnerService;
        }

        [HttpPost("rememberMe")]
        [SwaggerOperation(
            Summary = "Remember partner",
            Description = "Marks a partner as remembered by code")]
        [SwaggerResponse(200, "Successful response")]
        [SwaggerResponse(400, "Partner already exists")]
        public async Task<ActionResult> RememberMe([FromQuery, SwaggerParameter(Required = true)] string code)
        {
            var partner = await _partnerService.RememberPartnerByCodeAsync(code);
            return Ok(new { message = "Partner remembered successfully", partnerId = partner.Id });
        }
    }
}
