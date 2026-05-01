using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WiSave.Console.Execution;
using WiSave.Console.Infrastructure;
using WiSave.Console.Shell;

namespace WiSave.Console.Tests;

public sealed class ConsoleShellTests
{
    [Fact]
    public async Task RunAsync_executes_command_selected_by_number_and_then_exits()
    {
        var output = new TestConsoleOutput("1", "exit");
        var command = new SampleCommand();
        var services = CreateServices(output, command);
        var sut = services.GetRequiredService<IConsoleShell>();

        var exitCode = await sut.RunAsync(CancellationToken.None);

        Assert.Equal(0, exitCode);
        Assert.True(command.WasExecuted);
        Assert.Contains("Test Console", output.Lines);
    }

    [Fact]
    public async Task RunAsync_executes_command_selected_by_name_and_then_exits()
    {
        var output = new TestConsoleOutput("sample", "exit");
        var command = new SampleCommand();
        var services = CreateServices(output, command);
        var sut = services.GetRequiredService<IConsoleShell>();

        var exitCode = await sut.RunAsync(CancellationToken.None);

        Assert.Equal(0, exitCode);
        Assert.True(command.WasExecuted);
    }

    private static ServiceProvider CreateServices(IConsoleOutput output, IConsoleCommand command)
    {
        var services = new ServiceCollection();
        services.AddSingleton(output);
        services.AddSingleton(command);
        services.AddSingleton<ICommandCatalog, CommandCatalog>();
        services.AddSingleton<ICommandPrompter, CommandPrompter>();
        services.AddSingleton<ICommandRunner, CommandRunner>();
        services.AddSingleton<IConsoleShell, ConsoleShell>();
        services.AddSingleton(Options.Create(new ConsoleShellOptions { Title = "Test Console" }));
        return services.BuildServiceProvider();
    }

    private sealed class SampleCommand : IConsoleCommand
    {
        public bool WasExecuted { get; private set; }

        public string Name => "sample";

        public string Description => "Sample command.";

        public IReadOnlyList<CommandParameter> ParameterDefinitions => [];

        public Task<CommandResult> ExecuteAsync(CommandExecutionContext context, CancellationToken ct)
        {
            WasExecuted = true;
            return Task.FromResult(CommandResult.SuccessResult("sampled"));
        }
    }
}
