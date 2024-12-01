using ShortRouteOptimizerAPI.Models;
using System.Xml.Linq;

namespace ShortRouteOptimizerAPI.DataAccess
{
    /// <summary>
    /// Defines methods for accessing graph data.
    /// </summary>
    public interface IShortestPathRepository
    {
        /// <summary>
        /// Gets the graph with predefined nodes and edges.
        /// </summary>
        /// <returns>A list of nodes representing the graph.</returns>
        List<Node> GetGraph();
    }
}
