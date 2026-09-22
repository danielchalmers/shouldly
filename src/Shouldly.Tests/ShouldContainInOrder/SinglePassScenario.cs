namespace Shouldly.Tests.ShouldContainInOrder;

public class SinglePassScenario
{
    private int _enumerations;

    private IEnumerable<int> Numbers()
    {
        _enumerations++;
        yield return 1;
        yield return 2;
        yield return 3;
    }

    [Fact]
    public void LazySourceShouldFail()
    {
        var numbers = Numbers();
        Verify.ShouldFail(() =>
            numbers.ShouldContainInOrder([3, 1]));
        _enumerations.ShouldBe(1);
    }

    [Fact]
    public void LazySourceShouldPass()
    {
        var numbers = Numbers();
        numbers.ShouldContainInOrder([1, 3]);
        _enumerations.ShouldBe(1);
    }
}