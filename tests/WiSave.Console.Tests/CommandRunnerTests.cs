using Microsoft.Extensions.DependencyInjection;
using WiSave.Console.Execution;
using WiSave.Console.Shell;

namespace WiSave.Console.Tests;

public sealed class CommandRunnerTests
{
    [Fact]
    public async Task RunAsync_reports_missing_required_parameters()
    {
        var output = new TestConsoleOutput();
        var services = CreateServices(output, new RequiredParameterCommand());
        var sut = services.GetRequiredService<ICommandRunner>();

        var exitCode = await sut.RunAsync(new CommandInvocation("required", new Dictionary<string, string?>()), false, CancellationToken.None);

        Assert.Equal(1, exitCode);
        Assert.Contains("Missing required parameters:", output.Lines);
        Assert.Contains("  --name", output.Lines);
    }

    [Fact]
    public async Task RunAsync_resolves_command_names_case_insensitively()
    {
        var output = new TestConsoleOutput();
        var command = new RequiredParameterCommand();
        var services = CreateServices(output, command);
        var sut = services.GetRequiredService<ICommandRunner>();

        var exitCode = await sut.RunAsync(
            new CommandInvocation("REQUIRED", new Dictionary<string, string?> { ["name"] = "value" }),
            false,
            CancellationToken.None);

        Assert.Equal(0, exitCode);
        Assert.True(command.WasExecuted);
        Assert.Contains("OK: ran", output.Lines);
    }

    private static ServiceProvider CreateServices(IConsoleOutput output, params IConsoleCommand[] commands)
    {
        var services = new ServiceCollection();
        services.AddSingleton(output);
        services.AddSingleton<ICommandCatalog, CommandCatalog>();
        services.AddSingleton<ICommandPrompter, CommandPrompter>();
        services.AddSingleton<ICommandRunner, CommandRunner>();

        foreach (var command in commands)
        {
            services.AddSingleton(command);
        }

        return services.BuildServiceProvider();
    }

    private sealed class RequiredParameterCommand : IConsoleCommand
    {
        public bool WasExecuted { get; private set; }

        public string Name => "required";

        public string Description => "Requires a name.";

        public IReadOnlyList<CommandParameter> ParameterDefinitions { get; } =
        [
            new("name", "Name to use.", true)
        ];

        public Task<CommandResult> ExecuteAsync(CommandExecutionContext context, CancellationToken ct)
        {
            WasExecuted = true;
            return Task.FromResult(CommandResult.SuccessResult("ran"));
        }
    }
}
