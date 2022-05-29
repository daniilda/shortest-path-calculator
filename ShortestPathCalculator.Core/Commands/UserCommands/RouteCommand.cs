using ShortestPathCalculator.Core.Cache;
using ShortestPathCalculator.Core.Commands.Base;
using ShortestPathCalculator.Core.Essentials;

namespace ShortestPathCalculator.Core.Commands.UserCommands;

[Command("route")]
public class RouteCommand : ICommand
{
    private readonly INonPersistentCache<RouteMeta, Route> _cache;

    private const string ValidationException =
        "'/route' command shound have signature like: '/route id id'. Example: '/route 228 1337'";

    public RouteCommand() 
        => _cache = RoutesDictionaryCache.GetInstance();

    public void Execute(string[] input)
    {
        if (!Validate(input))
            return;
        var from = long.Parse(input[1]);
        var to =  long.Parse(input[2]);
        var meta = new RouteMeta
        {
            From = from,
            To = to
        };
        var result = _cache.TryGet(meta);
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write($"ROUTE: {result?.From} ----> {result?.To}\nDISTANCE: {result?.Distance}\nPATH: ");
        var path = result?.RoutePath.ToArray();
        for (var i = 0; i < path?.Length; i++)
        {
            Console.Write($"|{path?[i]}|");
            if (i != path?.Length - 1)
                Console.Write("->->->-");
        }
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkCyan;
    }

    private bool Validate(string[] input)
    {
        var result = input.Length >= 3 
               && long.TryParse(input[1], out _) 
               && long.TryParse(input[2], out _);
        if (!result)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ValidationException);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
        }

        return result;
    }
}