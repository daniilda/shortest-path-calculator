namespace ShortestPathCalculator.Core.Essentials;

public class Route
{
    public long From { get; set; }
    
    public long To { get; set; }

    public IEnumerable<long> RoutePath { get; set; } = new LinkedList<long>();
    
    public float Distance { get; set; }
}