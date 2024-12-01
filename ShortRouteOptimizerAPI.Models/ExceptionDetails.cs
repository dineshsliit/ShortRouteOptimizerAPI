using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShortRouteOptimizerAPI.Models
{
    /// <summary>
    /// Represents the details of an exception.
    /// </summary>
    public class ExceptionDetails
    {
        /// <summary>
        /// Gets or sets the status code of the exception.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the message of the exception.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Returns a string representation of the exception details in JSON format.
        /// </summary>
        /// <returns>A JSON string representation of the exception details.</returns>
        public override string ToString() => JsonSerializer.Serialize(this);

    }
}
