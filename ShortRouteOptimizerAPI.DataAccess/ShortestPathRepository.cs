using ShortRouteOptimizerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortRouteOptimizerAPI.DataAccess
{
    /// <summary>
    /// Repository for accessing graph data.
    /// </summary>
    public class ShortestPathRepository : IShortestPathRepository
    {
        /// <summary>
        /// Gets the graph with predefined nodes and edges.
        /// </summary>
        /// <returns>A list of nodes representing the graph.</returns>
        public List<Node> GetGraph()
        {
            var nodes = new Dictionary<string, Node>
            {
                 { "A", new Node("A") },
                 { "B", new Node("B") },
                 { "C", new Node("C") },
                 { "D", new Node("D") },
                 { "E", new Node("E") },
                 { "F", new Node("F") },
                 { "G", new Node("G") },
                 { "H", new Node("H") },
                 { "I", new Node("I") }
            };

            nodes["A"].Neighbors.Add((nodes["B"], 4));
            nodes["A"].Neighbors.Add((nodes["C"], 6));
            nodes["B"].Neighbors.Add((nodes["A"], 4));
            nodes["B"].Neighbors.Add((nodes["F"], 2));
            nodes["C"].Neighbors.Add((nodes["A"], 6));
            nodes["C"].Neighbors.Add((nodes["D"], 8));
            nodes["D"].Neighbors.Add((nodes["C"], 8));
            nodes["D"].Neighbors.Add((nodes["E"], 4));
            nodes["D"].Neighbors.Add((nodes["G"], 1));
            nodes["E"].Neighbors.Add((nodes["B"], 2));
            nodes["E"].Neighbors.Add((nodes["D"], 4));
            nodes["E"].Neighbors.Add((nodes["F"], 3));
            nodes["E"].Neighbors.Add((nodes["I"], 8));
            nodes["F"].Neighbors.Add((nodes["B"], 2));
            nodes["F"].Neighbors.Add((nodes["E"], 3));
            nodes["F"].Neighbors.Add((nodes["G"], 4));
            nodes["F"].Neighbors.Add((nodes["H"], 6));
            nodes["G"].Neighbors.Add((nodes["D"], 1));
            nodes["G"].Neighbors.Add((nodes["F"], 4));
            nodes["G"].Neighbors.Add((nodes["H"], 5));
            nodes["G"].Neighbors.Add((nodes["I"], 5));
            nodes["H"].Neighbors.Add((nodes["F"], 6));
            nodes["H"].Neighbors.Add((nodes["G"], 5));
            nodes["I"].Neighbors.Add((nodes["E"], 8));
            nodes["I"].Neighbors.Add((nodes["G"], 5));

            return new List<Node>(nodes.Values);
        }
    }
}
