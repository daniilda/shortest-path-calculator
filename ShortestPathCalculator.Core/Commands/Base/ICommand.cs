namespace ShortestPathCalculator.Core.Commands.Base;

public interface ICommand
{
    void Execute(string[] input);
}