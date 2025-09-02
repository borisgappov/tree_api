using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TreeApi.Models;
using TreeApi.Services;

namespace TreeApi.Controllers
{
    [ApiController]
    [Route("api.user.tree")]
    [SwaggerTag("Represents entire tree API")]
    public class TreeController : ControllerBase
    {
        private readonly ITreeService _treeService;

        public TreeController(ITreeService treeService)
        {
            _treeService = treeService;
        }

        [HttpPost("get")]
        [SwaggerOperation(
            Summary = "Get tree",
            Description = "Returns your entire tree. If your tree doesn't exist it will be created automatically.")]
        [SwaggerResponse(200, "Successful response", typeof(MNode))]
        public async Task<ActionResult<MNode>> Get([FromQuery, SwaggerParameter(Required = true)] string treeName)
        {
            var tree = await _treeService.GetTreeByNameAsync(treeName);
            return Ok(tree);
        }
    }
}
