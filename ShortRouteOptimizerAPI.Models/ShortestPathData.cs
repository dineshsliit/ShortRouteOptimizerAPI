using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortRouteOptimizerAPI.Models
{
    /// <summary>
    /// Represents the data for the shortest path in a graph.
    /// </summary>
    public class ShortestPathData
    {
        /// <summary>
        /// Gets or sets the list of node names in the order they are traversed.
        /// </summary>
        public List<string> NodeNames { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the total distance of the shortest path.
        /// </summary>
        public int Distance { get; set; }
    }
}
