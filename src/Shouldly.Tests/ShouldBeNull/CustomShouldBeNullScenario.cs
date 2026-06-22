using System.Runtime.CompilerServices;

namespace Shouldly.Tests.ShouldBeNull;

public class CustomShouldBeNullScenario
{
    // Reproduces #1092: a user-defined ShouldBeNull on a custom type was hijacked by
    // ShouldBeNullMessageGenerator (matched purely by method name), so the failure message
    // printed the expected sentinel instead of the actual value.
    [Fact]
    public void CustomShouldBeNullShowsActualNotExpectedSentinel()
    {
        var value = new CustomBox(42);

        Verify.ShouldFail(() =>
            value.ShouldBeNull());
    }
}

public readonly struct CustomBox(int value)
{
    public bool IsNull => false;

    public override string ToString() => $"CustomBox({value})";
}

[ShouldlyMethods]
public static class CustomBoxShouldExtensions
{
    public static void ShouldBeNull(this CustomBox actual, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
        => actual.AssertAwesomely(v => v.IsNull, actual, "the-expected-null-sentinel", customMessage, actualExpression: actualExpression);
}
