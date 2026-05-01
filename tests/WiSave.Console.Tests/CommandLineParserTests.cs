using WiSave.Console.Execution;

namespace WiSave.Console.Tests;

public sealed class CommandLineParserTests
{
    [Fact]
    public void Parse_returns_interactive_result_when_no_arguments_are_provided()
    {
        var sut = new CommandLineParser();

        var result = sut.Parse([]);

        Assert.True(result.IsInteractive);
        Assert.Null(result.Invocation);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public void Parse_returns_invocation_with_named_arguments()
    {
        var sut = new CommandLineParser();

        var result = sut.Parse(["db-migrate", "--connection-string", "Host=localhost"]);

        Assert.False(result.IsInteractive);
        Assert.NotNull(result.Invocation);
        Assert.Equal("db-migrate", result.Invocation.CommandName);
        Assert.Equal("Host=localhost", result.Invocation.Arguments["connection-string"]);
    }

    [Fact]
    public void Parse_treats_argument_without_value_as_boolean_true()
    {
        var sut = new CommandLineParser();

        var result = sut.Parse(["db-migrate", "--dry-run"]);

        Assert.False(result.IsInteractive);
        Assert.NotNull(result.Invocation);
        Assert.Equal("true", result.Invocation.Arguments["dry-run"]);
    }

    [Fact]
    public void Parse_rejects_unexpected_positional_argument_after_command_name()
    {
        var sut = new CommandLineParser();

        var result = sut.Parse(["db-migrate", "unexpected"]);

        Assert.False(result.IsInteractive);
        Assert.Null(result.Invocation);
        Assert.Equal("Unexpected argument 'unexpected'. Expected '--name value'.", result.ErrorMessage);
    }
}
