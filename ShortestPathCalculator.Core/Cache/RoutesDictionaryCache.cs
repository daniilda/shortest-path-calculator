using System.Collections.Concurrent;
using ShortestPathCalculator.Core.Essentials;

namespace ShortestPathCalculator.Core.Cache;

public class RoutesDictionaryCache : INonPersistentCache<RouteMeta, Route>
{
    # region Singleton
    
    private static INonPersistentCache<RouteMeta, Route>? _instance;

    public static INonPersistentCache<RouteMeta, Route> GetInstance() 
        => _instance ??= new RoutesDictionaryCache();
    
    # endregion
    
    private readonly IDictionary<RouteMeta, Route> _dictionary = new ConcurrentDictionary<RouteMeta, Route>();
    
    public void Add(RouteMeta key, Route data)
    {
        if (_dictionary.TryAdd(key, data)) 
            return;
        _dictionary.Remove(key);
        _dictionary.Add(key, data);
    }

    public void Delete(RouteMeta key)
    {
        _dictionary.Remove(key);
    }

    public Route? TryGet(RouteMeta key)
    {
        _dictionary.TryGetValue(key, out var value);
        return value;
    }

    public IReadOnlyDictionary<RouteMeta, Route>? AsReadOnly()
        => (IReadOnlyDictionary<RouteMeta, Route>) _dictionary;

    public int Count()
        => _dictionary.Count;
}