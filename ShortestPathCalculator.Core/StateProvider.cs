using ShortestPathCalculator.Core.Cache;
using ShortestPathCalculator.Core.Essentials;

namespace ShortestPathCalculator.Core;

public class StateProvider
{
    private readonly INonPersistentCache<RouteMeta, Route> _routesCache;
    private readonly INonPersistentCache<long, Point> _pointsCache;

    public StateProvider()
    {
        _routesCache = RoutesDictionaryCache.GetInstance();
        _pointsCache = PointsDictionaryCache.GetInstance();
    }

    public int RoutesCacheCount => _routesCache.Count();
    
    public string PointsState 
        => _pointsCache.Count() > 0 ? $"POINTS AMOUNT IS {_pointsCache.Count()}" : $"NOT INITIALIZED YET";

    public string RoutesState 
        => _routesCache.Count() > 0 ? $"ROUTES AMOUNT IS {_routesCache.Count()}" : $"NOT CALCULATED YET";

    public int PointsCacheCount => _pointsCache.Count();
}