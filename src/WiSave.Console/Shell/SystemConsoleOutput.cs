namespace WiSave.Console.Shell;

public sealed class SystemConsoleOutput : IConsoleOutput
{
    public void Write(string value) => global::System.Console.Write(value);

    public void WriteLine(string? value) => global::System.Console.WriteLine(value);

    public string? ReadLine() => global::System.Console.ReadLine();

    public void Clear() => global::System.Console.Clear();
}
