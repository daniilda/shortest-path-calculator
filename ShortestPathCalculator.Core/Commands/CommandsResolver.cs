using System.Reflection;
using ShortestPathCalculator.Core.Commands.Base;

namespace ShortestPathCalculator.Core.Commands;

public class CommandsResolver
{
    private readonly IDictionary<string, ICommand> _commands;

    public CommandsResolver()
    {
        var dictionary = new Dictionary<string, ICommand>();
        var commands = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.IsClass && type.GetInterfaces().Contains(typeof(ICommand)));
        
        foreach (var command in commands)
        {
            var commandData = command.GetCustomAttribute<CommandAttribute>();
            if (commandData is null)
                continue;
            var commandName = commandData.Command;
            var ctor = command.GetConstructor(Array.Empty<Type>());
            var obj = (ICommand?) ctor?.Invoke(null);
            if (obj is null)
                continue;
            if (!dictionary.TryAdd(commandName, obj))
                throw new Exception("There is command duplicates!");
        }

        _commands = dictionary;
    }

    public bool Resolve(string command)
    {
        var commandWithOutSlash = command[1..];
        var split = commandWithOutSlash.Split(" ");
        _commands.TryGetValue(split[0], out var value);
        if (value is null)
            return false;
        var trimmedSplit = split.Select(x => x.Trim()).Where(y => !string.IsNullOrEmpty(y) && !string.IsNullOrWhiteSpace(y)).ToArray();
        value.Execute(trimmedSplit);
        return true;
    }
}