using ShortRouteOptimizerAPI.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortRouteOptimizerAPI.Services
{
    public class NodesService : INodesService
    {
        private readonly IShortestPathRepository _shortestPathRepository;

        public NodesService(IShortestPathRepository shortestPathRepository)
        {
            _shortestPathRepository = shortestPathRepository;
        }

        /// <summary>
        /// Gets the names of nodes in the graph.
        /// </summary>
        public IEnumerable<string> GetNodesNamesFromGraph()
        {
            var graphNodes = _shortestPathRepository.GetGraph();

            if (graphNodes != null)
            {
                var nodeNames = graphNodes.Select(x => x.Name);

                return nodeNames;
            }
            else
                return Enumerable.Empty<string>();

        }
    }
}
