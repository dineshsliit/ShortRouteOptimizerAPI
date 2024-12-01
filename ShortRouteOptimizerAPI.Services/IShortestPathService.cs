using ShortRouteOptimizerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortRouteOptimizerAPI.Services
{
    /// <summary>
    /// Defines methods for calculating the shortest path between nodes in a graph.
    /// </summary>
    public interface IShortestPathService
    {
        /// <summary>
        /// Gets the shortest path between two nodes in the graph.
        /// </summary>
        /// <param name="fromNodeName">The name of the starting node.</param>
        /// <param name="toNodeName">The name of the destination node.</param>
        /// <returns>A <see cref="ShortestPathData"/> object containing the shortest path and distance.</returns>
        ShortestPathData GetShortestPathFromNodes(string fromNodeName, string toNodeName);

    }

}
