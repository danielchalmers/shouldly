namespace Shouldly;

public static partial class ShouldBeTestExtensions
{
    /// <summary>
    /// Asserts that a nullable double is non-null and equal to another double within the specified tolerance
    /// </summary>
    public static void ShouldBe(this double? actual, double expected, double tolerance, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
    {
        if (actual is null)
            throw new ShouldAssertException(new ExpectedActualShouldlyMessage($"{expected} (+/- {tolerance})", "null", customMessage, actualExpression: actualExpression).ToString());

        actual.Value.ShouldBe(expected, tolerance, customMessage, actualExpression);
    }

    /// <summary>
    /// Asserts that a nullable float is non-null and equal to another float within the specified tolerance
    /// </summary>
    public static void ShouldBe(this float? actual, float expected, double tolerance, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
    {
        if (actual is null)
            throw new ShouldAssertException(new ExpectedActualShouldlyMessage($"{expected} (+/- {tolerance})", "null", customMessage, actualExpression: actualExpression).ToString());

        actual.Value.ShouldBe(expected, tolerance, customMessage, actualExpression);
    }

    /// <summary>
    /// Asserts that a nullable decimal is non-null and equal to another decimal within the specified tolerance
    /// </summary>
    public static void ShouldBe(this decimal? actual, decimal expected, decimal tolerance, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
    {
        if (actual is null)
            throw new ShouldAssertException(new ExpectedActualShouldlyMessage($"{expected} (+/- {tolerance})", "null", customMessage, actualExpression: actualExpression).ToString());

        actual.Value.ShouldBe(expected, tolerance, customMessage, actualExpression);
    }
}
