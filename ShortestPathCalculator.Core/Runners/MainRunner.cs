using System.Xml;
using ShortestPathCalculator.Core.Commands;
using ShortestPathCalculator.Core.Essentials;
using ShortestPathCalculator.Core.Runners.Abstractions;

namespace ShortestPathCalculator.Core.Runners;

public class MainRunner : CommandRunner
{
    private readonly CommandsResolver _resolver;
    private readonly StateProvider _stateProvider;
    private bool _initialStartup = true;

    public MainRunner(CommandsResolver resolver, StateProvider stateProvider)
    {
        _resolver = resolver;
        _stateProvider = stateProvider;
    }

    public override void Run()
    {
        while (true)
        {
            if (_initialStartup)
                ShowApplicationInfo();
            var input = Console.ReadLine();

            _initialStartup = false;

            if (!Validate(input))
            {
                Console.Clear();
                ShowApplicationInfo(input);
                ShowErrorNotCommand(input);
                continue;
            }

            if (input == "/q")
                return;

            if (!_resolver.Resolve(input!))
            {
                Console.Clear();
                ShowApplicationInfo(input);
                ShowErrorNotCommand(input);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Press any key to continue...");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.ReadKey();
                Console.Clear();
                ShowApplicationInfo(input); 
            }
        }
    }

    private string ApplicationInfo =>
        @$"============================ SHORTEST PATH CALCULATOR =============================
========================= USING FLOYD–WARSHALL ALGORITHM ==========================

USE '/load FILENAME' TO LOAD POINTS FROM FILE TO CACHE (default is 'default.input')
USE '/calculate' TO CALCULATE ROUTES
USE '/route POINTID1 POINTID2' TO GET INFO ABOUT SHORTEST ROUTE
USE '/save FILENAME' TO SAVE ROUTES TO FILE (default is 'default.txt')
CURRENT STATE OF POINTS CACHE: {_stateProvider.PointsState}
CURRENT STATE OF ROUTES CACHE: {_stateProvider.RoutesState}

======================= MADE BY DANIIL KUZNETSOV (daniilda) =======================";

    private void ShowApplicationInfo(string? lastInput = null)
    {
        Console.WriteLine(ApplicationInfo);
        if (lastInput is not null)
            Console.WriteLine(lastInput);
    }

    private void ShowErrorNotCommand(string? input)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"'{input}' is not a command. Use /help (not implemented yet)");
        Console.ForegroundColor = ConsoleColor.DarkCyan;
    }
}