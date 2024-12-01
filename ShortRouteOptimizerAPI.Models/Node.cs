using System.Collections.Generic;

namespace ShortRouteOptimizerAPI.Models
{
    /// <summary>
    /// Represents a node in a graph.
    /// </summary>
    public class Node : IComparable<Node>
    {
        /// <summary>
        /// Gets or sets the name of the node.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of neighbors and their respective weights.
        /// </summary>
        public List<(Node neighbor, int weight)> Neighbors { get; set; }

        /// <summary>
        /// Gets or sets the list of neighbors and their respective weights.
        /// </summary>
        public Node(string name)
        {
            Name = name;
            Neighbors = new List<(Node, int)>();
        }

        /// <summary>
        /// Compares the current node with another node based on their names.
        /// </summary>
        /// <param name="other">The node to compare with the current node.</param>
        /// <returns>
        /// A value that indicates the relative order of the nodes being compared.
        /// The return value has these meanings:
        /// Less than zero: This node is less than the <paramref name="other"/> node.
        /// Zero: This node is equal to <paramref name="other"/>.
        /// Greater than zero: This node is greater than <paramref name="other"/>.
        /// </returns>
        public int CompareTo(Node other)
        {
            if (other == null)
                return 1;

            return this.Name.CompareTo(other.Name);

        }
    }
}
