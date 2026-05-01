using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using WiSave.Console.Execution;
using WiSave.Console.Shell;

namespace WiSave.Console.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWiSaveConsole(
        this IServiceCollection services,
        Action<ConsoleShellOptions>? configure = null,
        params Assembly[] commandAssemblies)
    {
        if (configure is null)
        {
            services.Configure<ConsoleShellOptions>(_ => { });
        }
        else
        {
            services.Configure(configure);
        }

        services.AddSingleton<IConsoleOutput, SystemConsoleOutput>();
        services.AddSingleton<ICommandCatalog, CommandCatalog>();
        services.AddSingleton<ICommandLineParser, CommandLineParser>();
        services.AddSingleton<ICommandPrompter, CommandPrompter>();
        services.AddSingleton<ICommandRunner, CommandRunner>();
        services.AddSingleton<IConsoleShell, ConsoleShell>();
        services.AddSingleton<IConsoleApplication, ConsoleApplication>();

        foreach (var assembly in commandAssemblies)
        {
            RegisterCommands(services, assembly);
        }

        return services;
    }

    private static void RegisterCommands(IServiceCollection services, Assembly assembly)
    {
        var commandTypes = assembly.GetTypes()
            .Where(type =>
                !type.IsAbstract &&
                !type.IsInterface &&
                typeof(IConsoleCommand).IsAssignableFrom(type))
            .ToArray();

        foreach (var commandType in commandTypes)
        {
            services.AddTransient(typeof(IConsoleCommand), commandType);
        }
    }
}
