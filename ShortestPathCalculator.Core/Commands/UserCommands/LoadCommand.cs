using ShortestPathCalculator.Core.Cache;
using ShortestPathCalculator.Core.Commands.Base;
using ShortestPathCalculator.Core.Essentials;
using ShortestPathCalculator.Core.Utils;

namespace ShortestPathCalculator.Core.Commands.UserCommands;

[Command("load")]
public class LoadCommand : ICommand
{
    private readonly INonPersistentCache<long, Point> _cache;

    public LoadCommand() 
        => _cache = PointsDictionaryCache.GetInstance();

    public void Execute(string[] input)
    {
        if (!Validate(input))
            return;
        var path = string.Empty;
        if (input.Length > 1)
        {
            var temp = input[1].Split(".");
            if (temp.Length > 1)
            {
                path = input[1];
            }
            else
            {
                path = input[1] + ".input";
            }
        }
        else
        {
            path = "default.input";
        }

        var file = File.ReadAllLines($"InputData/{path}");
        var filtered = file[1..];
        using var progress = new ProgressBar();
        var iterationCount = 0;
        foreach (var line in filtered)
        {
            var split = line.Split(" ");
            var id = long.Parse(split[0]);
            var neighborId = long.Parse(split[1]);
            var distance = int.Parse(split[2]);
            var neighborPoint = _cache.TryGet(neighborId);
            if (neighborPoint is null)
            {
                var neighborNewPoint = new Point(neighborId, Array.Empty<Neighbor>());
                _cache.Add(neighborId, neighborNewPoint);
            }
            var neighbor = new Neighbor
            {
                Id = neighborId,
                Distance = distance
            };
            var point = new Point(id, neighbor);
            var oldPoint = _cache.TryGet(id);
            if (oldPoint == null)
            {
                _cache.Add(id ,point);
                continue;
            }
            oldPoint.AppendNeighbor(neighbor);
            _cache.Add(id, oldPoint);
            progress.Report((double) iterationCount++ / filtered.Length);
        }
    }

    public bool Validate(string[] input)
    {
        if (input.Length < 1)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("'/load' command shound have signature like: '/load filename'. Example: '/load some.input'");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            return false;
        }

        if (input.Length == 1)
            return true;
    
        var result = true;
        try
        {
            var temp = input[1].Split(".");
            if (temp.Length > 1)
            {
                File.GetCreationTime(input[1]);
            }
            else
            {
                File.GetCreationTime(input[1] + ".input");
            }
        }
        catch
        {
            result = false;
        }

        if (result) 
            return result;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"There is not such file as '{input[1]}' in 'InputData' directory");
        Console.ForegroundColor = ConsoleColor.DarkCyan;

        return result;
    }
}