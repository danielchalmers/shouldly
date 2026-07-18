namespace Shouldly.Tests.ShouldContainInOrder;

public class ParamsScenario
{
    [Fact]
    public void ParamsScenarioShouldFail()
    {
        // The params overload resolves its message via stack walking, which renders differently per target framework, so this asserts the stable parts instead of an approved file.
        var ex = Should.Throw<ShouldAssertException>(() =>
            new[] { 1, 2, 3 }.ShouldContainInOrder(3, 1));
        ex.Message.ShouldContain("should contain in order");
        ex.Message.ShouldContain("[3, 1]");
    }

    [Fact]
    public void ShouldPass()
    {
        new[] { 1, 2, 3, 4, 5 }.ShouldContainInOrder(1, 3, 5);
        new[] { 1, 2, 3, 4 }.ShouldContainInConsecutiveOrder(2, 3);
    }
}