using System.Numerics;
using ShortestPathCalculator.Core.Cache;
using ShortestPathCalculator.Core.Essentials;
using ShortestPathCalculator.Core.Utils;

namespace ShortestPathCalculator.Core.Core;

public class Calculator
{
    private const int NoEdge = int.MaxValue / 2 - 1;

    private readonly INonPersistentCache<long, Point> _pointsCache;
    private readonly INonPersistentCache<RouteMeta, Route> _routesCache;

    private readonly IDictionary<long, long> _bufferMatrix = new Dictionary<long, long>();
    private readonly IDictionary<long, long> _bufferReverseMatrix = new Dictionary<long, long>();

    public Calculator()
    {
        _pointsCache = PointsDictionaryCache.GetInstance();
        _routesCache = RoutesDictionaryCache.GetInstance();
    }

    public void Calculate(ProgressBar progress)
    {
        var readOnlyPoints = _pointsCache.AsReadOnly();
        if (readOnlyPoints is null)
            return;

        var size = readOnlyPoints.Count;
        var matrix = new int[size * size];
        var routesMatrix = new int[size * size];
        for (var i = 0; i < routesMatrix.Length; i++) 
            routesMatrix[i] = NoEdge;
        for (var i = 0; i < matrix.Length; i++)
            matrix[i] = NoEdge;
        var counter = 0;
        foreach (var (key, value) in readOnlyPoints)
        {
            _bufferMatrix.Add(counter, key);
            _bufferReverseMatrix.Add(key, counter++);
        }
            

        foreach (var (key, value) in readOnlyPoints)
        {
            var neighbors = value.Neighbors;
            foreach (var neighbor in neighbors)
            {
                var routeMeta = new RouteMeta
                {
                    From = key,
                    To = neighbor.Id
                };
                var route = new Route
                {
                    From = key,
                    To = neighbor.Id,
                    Distance = neighbor.Distance
                };
                _routesCache.Add(routeMeta, route);
                if (!_bufferMatrix.TryGetValue(neighbor.Id, out var col))
                    continue;
                var row = _bufferMatrix[key];
                matrix[row * size + col] = neighbor.Distance;
            }
        }

        var processedRoutes = Calculate(matrix, size, routesMatrix, progress);
        RebuildRoute(processedRoutes, size);
    }

    private int[] Calculate(int[] matrix, int sz, int[] routesMatrix, ProgressBar progress)
    {
        for (var k = 0; k < sz; ++k)
        {
            for (var i = 0; i < sz; ++i)
            {
                if (matrix[i * sz + k] == NoEdge)
                {
                    continue;
                }
                for (var j = 0; j < sz; ++j)
                {
                    var distance = matrix[i * sz + k] + matrix[k * sz + j];
                    if (matrix[i * sz + j] > distance)
                    {
                        matrix[i * sz + j] = distance;
                        routesMatrix[i * sz + j] = k;
                        var meta = new RouteMeta
                        {
                            From = _bufferMatrix[i],
                            To = _bufferMatrix[j]
                        };
                        var route = new Route
                        {
                            From = meta.From,
                            To = meta.To,
                            Distance = distance
                        };
                        _routesCache.Add(meta, route);
                    }
                }
            }
            progress.Report((double) k / sz);
        }
        return routesMatrix;
    }
    
    public void RebuildRoute(int[] routes, int sz)
    {
        var routesReadOnly = _routesCache.AsReadOnly()!;
        foreach (var (key, value) in routesReadOnly)
        {
            var from = key.From;
            var to = key.To;
            var fromRev = _bufferReverseMatrix[from];
            var toRev = _bufferReverseMatrix[to];
            var route = new LinkedList<long>();

            var point = routes[fromRev * sz + toRev];
            while (point != NoEdge) 
            {
                route.AddFirst(_bufferMatrix[point]);
                if (route.Count() > 20)
                    Console.WriteLine("rer");
                point = routes[fromRev * sz + point];
            }

            route.AddFirst(from);
            route.AddLast(to);
            var newRoute = new Route
            {
                From = key.From,
                To = key.To,
                Distance = value.Distance,
                RoutePath = route
            };
            _routesCache.Add(key, newRoute);
        }
    }
}