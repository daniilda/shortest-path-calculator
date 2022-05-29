using System.Collections.Concurrent;
using ShortestPathCalculator.Core.Essentials;

namespace ShortestPathCalculator.Core.Cache;

public class PointsDictionaryCache : INonPersistentCache<long, Point>
{
    # region Singleton
    
    private static INonPersistentCache<long, Point>? _instance;

    public static INonPersistentCache<long, Point> GetInstance() 
        => _instance ??= new PointsDictionaryCache();
    
    # endregion

    private readonly IDictionary<long, Point> _dictionary = new ConcurrentDictionary<long, Point>();

    public void Add(long key, Point data)
    {
        foreach (var neighbor in data.Neighbors)
        {
            _dictionary.TryGetValue(neighbor.Id, out var value);
            if (value is null)
                continue;
            if (value.Id == key)
                throw new ArgumentException("Graphs should be acyclic. Found two paths between two roads.");
        }

        if (_dictionary.TryAdd(key, data)) 
            return;
        
        _dictionary.Remove(key);
        _dictionary.Add(key, data);
    }

    public void Delete(long key)
    {
        _dictionary.Remove(key);
    }

    public Point? TryGet(long key)
    {
        _dictionary.TryGetValue(key, out var value);
        return value;
    }

    public IReadOnlyDictionary<long, Point>? AsReadOnly()
        => (IReadOnlyDictionary<long, Point>?) _dictionary;

    public int Count()
        => _dictionary.Count;
}