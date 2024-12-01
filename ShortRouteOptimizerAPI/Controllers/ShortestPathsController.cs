using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Serilog;
using ShortRouteOptimizerAPI.DataAccess;
using ShortRouteOptimizerAPI.Models;
using ShortRouteOptimizerAPI.Services;
using System.Collections.Generic;

namespace ShortRouteOptimizerAPI.Controllers
{
    /// <summary>
    /// API controller for handling shortest path requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ShortestPathsController : ControllerBase
    {
        private readonly IShortestPathService _shortestPathService;
        private readonly IMemoryCache _cache;
        private readonly CacheSettings _cacheSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortestPathsController"/> class.
        /// </summary>
        /// <param name="shortestPathService">The service for calculating the shortest path.</param>
        public ShortestPathsController(IShortestPathService shortestPathService, IOptions<CacheSettings> cacheSettings, IMemoryCache cache)
        {
            _shortestPathService = shortestPathService;
            _cacheSettings = cacheSettings.Value;
            _cache = cache;
        }

        /// <summary>
        /// Gets the shortest path between two nodes.
        /// </summary>
        /// <param name="from">The name of the starting node.</param>
        /// <param name="to">The name of the destination node.</param>
        /// <returns>A <see cref="ShortestPathData"/> object containing the shortest path and distance.</returns>
        [HttpGet("{from}/{to}")]
        public ActionResult<ShortestPathData> GetShortestPath(string from, string to)
        {
            if (from == null || to == null)
                return BadRequest("Invalid input parameters");

            var cacheKey = string.Format(_cacheSettings.ShortestPathCacheKey, from, to);

            if(_cache.TryGetValue(cacheKey, out ShortestPathData cacheData))
            {
                Log.Information("Shortest path from {from} to {to} retrieved from cache", from, to);

                return Ok(cacheData);
            }

            try
            {
                var shortestPathData = _shortestPathService.GetShortestPathFromNodes(from, to);

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(_cacheSettings.CacheDurationMinutes));
                
                _cache.Set(cacheKey, shortestPathData, cacheEntryOptions);

                Log.Information("Shortest path from {from} to {to} retrieved successfully", from, to);

                return Ok(shortestPathData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error retrieving shortest path from {from} to {to}", from, to);

                return BadRequest(ex.Message);
            }
        }
    }
}
