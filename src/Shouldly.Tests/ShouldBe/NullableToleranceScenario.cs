namespace Shouldly.Tests.ShouldBe;

public class NullableToleranceScenario
{
    [Fact]
    public void Nullable_double_within_tolerance_passes()
    {
        double? value = 2.5;
        value.ShouldBe(2.4, 0.2);
    }

    [Fact]
    public void Nullable_double_outside_tolerance_fails()
    {
        double? value = 2.5;
        Should.Throw<ShouldAssertException>(() => value.ShouldBe(2.0, 0.1));
    }

    [Fact]
    public void Null_nullable_double_fails()
    {
        double? value = null;
        Should.Throw<ShouldAssertException>(() => value.ShouldBe(2.0, 0.1));
    }

    [Fact]
    public void Nullable_float_and_decimal_overloads()
    {
        float? f = 1.0f;
        f.ShouldBe(1.05f, 0.1);

        decimal? d = 1.00m;
        d.ShouldBe(1.02m, 0.05m);
    }
}
