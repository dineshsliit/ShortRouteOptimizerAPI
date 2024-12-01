/// <summary>
/// Represents the settings for caching shortest path data.
/// </summary>
public class CacheSettings
{
    /// <summary>
    /// Gets or sets the cache key for storing the shortest path data.
    /// </summary>
    public string ShortestPathCacheKey { get; set; }

    /// <summary>
    /// Gets or sets the duration in minutes for which the cache is valid.
    /// </summary>
    public int CacheDurationMinutes { get; set; }
}



