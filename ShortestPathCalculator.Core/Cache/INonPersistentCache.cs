namespace ShortestPathCalculator.Core.Cache;

public interface INonPersistentCache<TKey, TData>
{
    void Add(TKey key, TData data);

    void Delete(TKey key);

    TData? TryGet(TKey key);

    IReadOnlyDictionary<TKey, TData>? AsReadOnly();

    int Count();
}