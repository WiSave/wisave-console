namespace WiSave.Console.Shell;

public interface IConsoleOutput
{
    void Write(string value);
    void WriteLine(string? value);
    string? ReadLine();
    void Clear();
}
