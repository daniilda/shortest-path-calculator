namespace ShortestPathCalculator.Core.Runners.Abstractions;

public abstract class CommandRunner : IRunner
{
    private const char CommandStarter = '/';
    public abstract void Run();

    protected static bool Validate(string? command)
    {
        if (string.IsNullOrEmpty(command) || string.IsNullOrWhiteSpace(command))
            return false;
        
        return command[0] == CommandStarter;
    }
}