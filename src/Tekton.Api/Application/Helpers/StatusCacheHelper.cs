using Microsoft.Extensions.Caching.Memory;

namespace Tekton.Api.Application.Helpers;
public class StatusCacheHelper : IStatusCacheHelper
{
    private readonly IMemoryCache _memoryCache;

    public StatusCacheHelper(IMemoryCache cache)
    {
        _memoryCache = cache;

        CacheStatuses();
    }

    private void CacheStatuses()
    {
        var statuses = new Dictionary<bool, string>()
            {
                {true, "Active"},
                {false, "Inactive"}
            };

        _memoryCache.Set("StatusDictionary", statuses, TimeSpan.FromMinutes(5));
    }

    public string GetStatusName(bool statusKey)
    {
        if (_memoryCache.TryGetValue("StatusDictionary", out Dictionary<bool, string> statuses))
        {
            if (statuses.TryGetValue(statusKey, out string statusName))
                return statusName;
        }

        return string.Empty;
    }
}

public interface IStatusCacheHelper
{
    string GetStatusName(bool statusKey);
}
