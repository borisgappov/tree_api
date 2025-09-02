using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TreeApi.Models;
using TreeApi.Services;

namespace TreeApi.Controllers
{
    [ApiController]
    [Route("api.user.tree.node")]
    [SwaggerTag("Represents tree node API")]
    public class TreeNodeController : ControllerBase
    {
        private readonly ITreeService _treeService;

        public TreeNodeController(ITreeService treeService)
        {
            _treeService = treeService;
        }

        [HttpPost("create")]
        [SwaggerOperation(
            Summary = "Create a new node",
            Description = "Create a new node in your tree. You must to specify a parent node ID that belongs to your tree. A new node name must be unique across all siblings.")]
        [SwaggerResponse(200, "Successful response")]
        public async Task<ActionResult> Create(
            [FromQuery, SwaggerParameter(Required = true)] string treeName,
            [FromQuery, SwaggerParameter(Required = true)] long parentNodeId,
            [FromQuery, SwaggerParameter(Required = true)] string nodeName)
        {
            var nodeModel = new MNode { Name = nodeName };
            await _treeService.CreateNodeByNameAsync(treeName, nodeModel, parentNodeId);
            return Ok();
        }

        [HttpPost("delete")]
        [SwaggerOperation(
            Summary = "Delete a node",
            Description = "Delete an existing node in your tree. You must specify a node ID that belongs your tree.")]
        [SwaggerResponse(200, "Successful response")]
        public async Task<ActionResult> Delete(
            [FromQuery, SwaggerParameter(Required = true)] string treeName,
            [FromQuery, SwaggerParameter(Required = true)] long nodeId)
        {
            await _treeService.DeleteNodeByNameAsync(treeName, nodeId);
            return Ok();
        }

        [HttpPost("rename")]
        [SwaggerOperation(
            Summary = "Rename a node",
            Description = "Rename an existing node in your tree. You must specify a node ID that belongs your tree. A new name of the node must be unique across all siblings.")]
        [SwaggerResponse(200, "Successful response")]
        public async Task<ActionResult> Rename(
            [FromQuery, SwaggerParameter(Required = true)] string treeName,
            [FromQuery, SwaggerParameter(Required = true)] long nodeId,
            [FromQuery, SwaggerParameter(Required = true)] string newNodeName)
        {
            var nodeModel = new MNode { Name = newNodeName };
            await _treeService.UpdateNodeByNameAsync(treeName, nodeId, nodeModel);
            return Ok();
        }
    }
}
