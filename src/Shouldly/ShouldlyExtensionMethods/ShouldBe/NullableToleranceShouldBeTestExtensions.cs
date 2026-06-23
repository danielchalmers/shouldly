namespace Shouldly;

/// <summary>
/// Tolerance-based equality for nullable floating point receivers. Shouldly only ships non-nullable
/// tolerance overloads (double/float/decimal); FluentAssertions' BeApproximately accepts nullable
/// receivers too. These overloads let <c>(double?)x.ShouldBe(expected, tolerance)</c> compile and fail
/// cleanly when the value is null.
/// </summary>
public static partial class ShouldBeTestExtensions
{
    /// <summary>
    /// Asserts that a nullable double is non-null and equal to the expected value within the tolerance.
    /// </summary>
    public static void ShouldBe(this double? actual, double expected, double tolerance, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
    {
        if (actual is null)
            throw new ShouldAssertException(new ExpectedActualShouldlyMessage($"{expected} (+/- {tolerance})", "null", customMessage, actualExpression: actualExpression).ToString());

        actual.Value.ShouldBe(expected, tolerance, customMessage, actualExpression);
    }

    /// <summary>
    /// Asserts that a nullable float is non-null and equal to the expected value within the tolerance.
    /// </summary>
    public static void ShouldBe(this float? actual, float expected, double tolerance, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
    {
        if (actual is null)
            throw new ShouldAssertException(new ExpectedActualShouldlyMessage($"{expected} (+/- {tolerance})", "null", customMessage, actualExpression: actualExpression).ToString());

        actual.Value.ShouldBe(expected, tolerance, customMessage, actualExpression);
    }

    /// <summary>
    /// Asserts that a nullable decimal is non-null and equal to the expected value within the tolerance.
    /// </summary>
    public static void ShouldBe(this decimal? actual, decimal expected, decimal tolerance, string? customMessage = null,
        [CallerArgumentExpression(nameof(actual))] string? actualExpression = null)
    {
        if (actual is null)
            throw new ShouldAssertException(new ExpectedActualShouldlyMessage($"{expected} (+/- {tolerance})", "null", customMessage, actualExpression: actualExpression).ToString());

        actual.Value.ShouldBe(expected, tolerance, customMessage, actualExpression);
    }
}
