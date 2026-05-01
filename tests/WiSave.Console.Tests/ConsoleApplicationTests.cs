using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WiSave.Console.Execution;
using WiSave.Console.Infrastructure;
using WiSave.Console.Shell;

namespace WiSave.Console.Tests;

public sealed class ConsoleApplicationTests
{
    [Fact]
    public async Task RunAsync_prints_available_commands_for_help()
    {
        var output = new TestConsoleOutput();
        var services = new ServiceCollection();
        services.AddSingleton<IConsoleOutput>(output);
        services.AddSingleton<IConsoleCommand, SampleCommand>();
        services.AddSingleton<ICommandCatalog, CommandCatalog>();
        services.AddSingleton<ICommandLineParser, CommandLineParser>();
        services.AddSingleton<ICommandPrompter, CommandPrompter>();
        services.AddSingleton<ICommandRunner, CommandRunner>();
        services.AddSingleton<IConsoleShell, ConsoleShell>();
        services.AddSingleton<IConsoleApplication, ConsoleApplication>();
        services.AddSingleton(Options.Create(new ConsoleShellOptions()));
        await using var provider = services.BuildServiceProvider();
        var sut = provider.GetRequiredService<IConsoleApplication>();

        var exitCode = await sut.RunAsync(["help"], CancellationToken.None);

        Assert.Equal(0, exitCode);
        Assert.Contains("Available commands:", output.Lines);
        Assert.Contains("  sample - Sample command.", output.Lines);
        Assert.Contains("Run without arguments to start the interactive shell.", output.Lines);
    }

    private sealed class SampleCommand : IConsoleCommand
    {
        public string Name => "sample";

        public string Description => "Sample command.";

        public IReadOnlyList<CommandParameter> ParameterDefinitions => [];

        public Task<CommandResult> ExecuteAsync(CommandExecutionContext context, CancellationToken ct)
            => Task.FromResult(CommandResult.SuccessResult("sampled"));
    }
}
