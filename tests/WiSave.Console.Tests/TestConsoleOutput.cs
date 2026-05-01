using WiSave.Console.Shell;

namespace WiSave.Console.Tests;

internal sealed class TestConsoleOutput(params string[] inputs) : IConsoleOutput
{
    private readonly Queue<string> inputQueue = new(inputs);
    private readonly List<string> lines = [];

    public IReadOnlyList<string> Lines => lines;

    public void Write(string value)
    {
        if (value.Length > 0)
        {
            lines.Add(value);
        }
    }

    public void WriteLine(string? value)
        => lines.Add(value ?? string.Empty);

    public string? ReadLine()
        => inputQueue.Count > 0 ? inputQueue.Dequeue() : null;

    public void Clear()
    {
        lines.Clear();
    }
}
