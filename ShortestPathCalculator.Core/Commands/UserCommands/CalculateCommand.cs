using ShortestPathCalculator.Core.Cache;
using ShortestPathCalculator.Core.Commands.Base;
using ShortestPathCalculator.Core.Core;
using ShortestPathCalculator.Core.Essentials;
using ShortestPathCalculator.Core.Utils;

namespace ShortestPathCalculator.Core.Commands.UserCommands;

[Command("calculate")]
public class CalculateCommand : ICommand
{

    private readonly INonPersistentCache<long, Point> _cache;
    
    private const string ValidationException =
        "'/calculate' command shound be called after points cache initialized. Try '/read' command first";

    public CalculateCommand() 
        => _cache = PointsDictionaryCache.GetInstance();

    public void Execute(string[] input)
    {
        if (!Validate(input))
            return;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("STARTING ROUTES CALCULATION");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        var calculator = new Calculator();
        using var progress = new ProgressBar();
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        calculator.Calculate(progress);
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine();
        Console.WriteLine("ROUTES CALCULATION IS COMPLETE!");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
    }

    private bool Validate(string[] input)
    {
       var result = _cache.Count() > 0;
       if (!result)
       {
           Console.ForegroundColor = ConsoleColor.Red;
           Console.WriteLine(ValidationException);
           Console.ForegroundColor = ConsoleColor.DarkCyan;
       }
       return result;
    }
}