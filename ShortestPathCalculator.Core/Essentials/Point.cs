using System.Numerics;

namespace ShortestPathCalculator.Core.Essentials;

public class Point
{
    public Point(long id, params Neighbor[] neighbors) : this(id, string.Empty, new Vector2(0, 0), neighbors)
    {
    }

    public Point(long id, string address, float x, float y, params Neighbor[] neighbors) : this(id, address, new Vector2(x, y), neighbors)
    {
    }

    public Point(long id, string address, Vector2 location, params Neighbor[] neighbors)
    {
        Id = id;
        Address = address;
        Location = location;
        Neighbors = neighbors.ToList();
    }

    public long Id { get; }
    
    public string Address { get; }
    
    public Vector2 Location { get; }
    
    public IList<Neighbor> Neighbors { get; }

    public void AppendNeighbor(Neighbor neighbor)
        => Neighbors.Add(neighbor);
}