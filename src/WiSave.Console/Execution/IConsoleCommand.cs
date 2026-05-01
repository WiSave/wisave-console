namespace WiSave.Console.Execution;

public interface IConsoleCommand
{
    string Name { get; }

    string Description { get; }

    IReadOnlyList<CommandParameter> ParameterDefinitions { get; }

    Task<CommandResult> ExecuteAsync(CommandExecutionContext context, CancellationToken ct);
}
