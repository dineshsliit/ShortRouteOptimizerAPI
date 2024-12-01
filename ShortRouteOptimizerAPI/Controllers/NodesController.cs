using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ShortRouteOptimizerAPI.Services;

namespace ShortRouteOptimizerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NodesController : ControllerBase
    {
        private readonly INodesService _nodeService;

        public NodesController(INodesService nodesService)
        {
            _nodeService = nodesService;
        }

        /// <summary>
        /// Gets the node names.
        /// </summary>
        /// <returns> Node Names.</returns>
        [HttpGet("GetNodes")]
        public ActionResult<IEnumerable<string>> GetNodes()
        {
            try
            {
                var nodes = _nodeService.GetNodesNamesFromGraph();

                Log.Information("{@nodes} retrieved successfully", nodes);

                return Ok(nodes);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving nodes");

                return BadRequest(ex.Message);
            }
            
        }
    }
}
