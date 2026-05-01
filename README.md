# WiSave.Console

Reusable console command infrastructure for WiSave services.

The package provides:

- command contracts and parameter metadata
- command-line parsing
- command catalog and runner
- interactive shell hosting
- console output abstraction for tests
- dependency injection registration helpers

## Usage

```csharp
services.AddWiSaveConsole(
    options => options.Title = "WiSave Expenses Console",
    typeof(Program).Assembly);
```

Commands implement `IConsoleCommand` and are discovered from the assemblies passed to `AddWiSaveConsole`.
