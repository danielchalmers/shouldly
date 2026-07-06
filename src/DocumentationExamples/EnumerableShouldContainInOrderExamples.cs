public class EnumerableShouldContainInOrderExamples
{
    ITestOutputHelper _testOutputHelper;

    public EnumerableShouldContainInOrderExamples(ITestOutputHelper testOutputHelper) =>
        _testOutputHelper = testOutputHelper;

    [Fact]
    public void ShouldContainInOrder()
    {
        DocExampleWriter.Document(
            () =>
            {
                var homer = new Person { Name = "Homer" };
                var moe = new Person { Name = "Moe" };
                var barney = new Person { Name = "Barney" };
                var arrivals = new List<Person> { moe, homer, barney };

                arrivals.ShouldContainInOrder(new List<Person> { homer, moe });
            },
            _testOutputHelper);
    }

    [Fact]
    public void ShouldContainInConsecutiveOrder()
    {
        DocExampleWriter.Document(
            () =>
            {
                var homer = new Person { Name = "Homer" };
                var marge = new Person { Name = "Marge" };
                var bart = new Person { Name = "Bart" };
                var lisa = new Person { Name = "Lisa" };
                var lineup = new List<Person> { homer, marge, bart, lisa };

                lineup.ShouldContainInConsecutiveOrder(new List<Person> { marge, lisa });
            },
            _testOutputHelper);
    }
}