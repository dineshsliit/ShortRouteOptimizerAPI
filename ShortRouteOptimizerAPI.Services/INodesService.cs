using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortRouteOptimizerAPI.Services
{
    public interface INodesService
    {
        /// <summary>
        /// Gets the nodes in the graph.
        /// </summary>
        IEnumerable<string> GetNodesNamesFromGraph();
    }
}
