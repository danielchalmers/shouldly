namespace Shouldly.MessageGenerators;

class ShouldHaveCountMessageGenerator : ShouldlyMessageGenerator
{
    public override bool CanProcess(IShouldlyAssertionContext context) =>
        context.ShouldMethod.StartsWith("ShouldHaveCount", StringComparison.Ordinal);

    public override string GenerateErrorMessage(IShouldlyAssertionContext context)
    {
        var codePart = context.CodePart;
        var expected = context.Expected.ToStringAwesomely();
        var count = (context.Actual as IEnumerable)?.Cast<object?>().Count() ?? 0;
        var should = context.ShouldMethod.PascalToSpaced();

        return
            $"""
             {codePart}
                 {should}
             {expected}
                 but had {count}
             {context.Actual.ToStringAwesomely()}
             """;
    }
}
