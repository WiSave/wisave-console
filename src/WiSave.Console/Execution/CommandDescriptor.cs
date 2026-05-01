namespace WiSave.Console.Execution;

public sealed record CommandDescriptor(
    string Name,
    string Description,
    IReadOnlyList<CommandParameter> Parameters);
