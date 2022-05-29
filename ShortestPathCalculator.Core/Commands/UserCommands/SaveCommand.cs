using ShortestPathCalculator.Core.Cache;
using ShortestPathCalculator.Core.Commands.Base;
using ShortestPathCalculator.Core.Essentials;

namespace ShortestPathCalculator.Core.Commands.UserCommands;

[Command("save")]
public class SaveCommand : ICommand
{
    private readonly INonPersistentCache<RouteMeta, Route> _cache;

    public SaveCommand()
        => _cache = RoutesDictionaryCache.GetInstance();

    public void Execute(string[] input)
    {
        var fileName = "";
        switch (input.Length)
        {
            case 1:
                fileName = "default.txt";
                WriteToFile(fileName);
                break;
            case > 1:
                var file = input[1];
                var split = file.Split(".");
                var temp = split.Length;
                if (temp == 1)
                    fileName = $"{file}.txt";
                else if (temp == 2)
                {
                    fileName = file;
                }
                WriteToFile(fileName);
                break;
        }
    }

    private void WriteToFile(string path)
    {
        foreach (var (key, value) in _cache.AsReadOnly()!)
        {
            var routePath = "";
            var rPath = value.RoutePath.ToArray();
            for (var i = 0; i < rPath.Length; i++)
            {
                routePath += $"[{rPath[i]}]";
                if (i != rPath.Length - 1)
                    routePath += " ----> ";
            }

            var output = new string[1];
            output[0] = $"ROUTE: {key.From} ---> {key.To} | DISTANCE: {value.Distance} | PATH: {routePath}";
            File.AppendAllLines(path, output);
        }
    }
}