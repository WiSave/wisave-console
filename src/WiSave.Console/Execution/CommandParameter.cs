namespace WiSave.Console.Execution;

public sealed record CommandParameter(
    string Name,
    string Description,
    bool Required,
    string? DefaultValue = null);
