using ShortestPathCalculator.Core.Commands;
using ShortestPathCalculator.Core.Runners;

namespace ShortestPathCalculator.Core;

public static class Startup
{
    #region Singleton

    private static MainRunner? _instance;
    
    public static MainRunner Runner => 
        _instance ??= Initialize();

    #endregion
    
    private static MainRunner Initialize()
    {
        ConfigureConsole();
        
        // ALL REQUIRED SERVICES TO HERE;
        var commandResolver = new CommandsResolver();
        var stateProvider = new StateProvider();

        return new MainRunner(commandResolver, stateProvider);
    }

    private static void ConfigureConsole()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
    }
}