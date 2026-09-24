namespace Shouldly.MessageGenerators;

class ShouldContainInOrderMessageGenerator : ShouldlyMessageGenerator
{
    public override bool CanProcess(IShouldlyAssertionContext context) =>
        context.ShouldMethod is "ShouldContainInOrder" or "ShouldContainInConsecutiveOrder";

    public override string GenerateErrorMessage(IShouldlyAssertionContext context)
    {
        var header = $"""
                      {context.CodePart}
                          {context.ShouldMethod.PascalToSpaced()}
                      {context.Expected.ToStringAwesomely()}
                      """;

        if (context.Actual == null)
        {
            return $"""
                    {header}
                        but was
                    null
                    """;
        }

        var actual = context.Actual.ToStringAwesomely();
        if (context is not ShouldlyAssertionContext { ContainInOrderMismatch: { } mismatch })
        {
            return $"""
                    {header}
                        but was actually
                    {actual}
                    """;
        }

        var but = context.CodePartMatchesActual
            ? "but"
            : $"""
               but was actually
               {actual}
                   and
               """;

        if (mismatch.ActualIndex < 0)
        {
            return $"""
                    {header}
                        {but} expected item at index {mismatch.ExpectedIndex}
                    {mismatch.ExpectedItem.ToStringAwesomely()}
                        was not found
                    """;
        }

        if (context.ShouldMethod == "ShouldContainInConsecutiveOrder")
        {
            // Show the matched run as it appears in the actual values, which can differ from the expected ones under a custom comparer.
            var longestMatch = (context.Actual as IEnumerable ?? Enumerable.Empty<object>()).Cast<object?>().Skip(mismatch.ActualIndex).Take(mismatch.ExpectedIndex).ToList();
            return $"""
                    {header}
                        {but} the longest consecutive match was
                    {longestMatch.ToStringAwesomely()}
                        at index {mismatch.ActualIndex}, which was not followed by
                    {mismatch.ExpectedItem.ToStringAwesomely()}
                    """;
        }

        return $"""
                {header}
                    {but} expected item at index {mismatch.ExpectedIndex}
                {mismatch.ExpectedItem.ToStringAwesomely()}
                    was not found after the previous match at index {mismatch.ActualIndex}
                """;
    }
}