# ContainInOrder

These check that a given sequence of values appears within the enumerable. To check that the enumerable itself is sorted, use `ShouldBeInOrder` instead.

## ShouldContainInOrder

Asserts that the expected values appear in the enumerable in the same relative order. The matched values do not need to be adjacent, but each expected value needs its own element, so `[1, 1]` requires two 1s.

<!-- snippet: EnumerableShouldContainInOrderExamples.ShouldContainInOrder.codeSample.approved.cs -->
<a id='snippet-EnumerableShouldContainInOrderExamples.ShouldContainInOrder.codeSample.approved.cs'></a>
```cs
var homer = new Person { Name = "Homer" };
var moe = new Person { Name = "Moe" };
var barney = new Person { Name = "Barney" };
var arrivals = new List<Person> { moe, homer, barney };
arrivals.ShouldContainInOrder([homer, moe]);
```
<sup><a href='/src/DocumentationExamples/CodeExamples/EnumerableShouldContainInOrderExamples.ShouldContainInOrder.codeSample.approved.cs#L1-L5' title='Snippet source file'>snippet source</a> | <a href='#snippet-EnumerableShouldContainInOrderExamples.ShouldContainInOrder.codeSample.approved.cs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

**Exception**

<!-- include: EnumerableShouldContainInOrderExamples.ShouldContainInOrder.exceptionText.approved.txt -->
```
arrivals
    should contain in order
[Homer, Moe]
    but was actually
[Moe, Homer, Barney]
    and expected item at index 1
Moe
    was not found after the previous match at index 1
```
<!-- endInclude -->

## ShouldContainInConsecutiveOrder

Asserts that the expected values appear in the enumerable as a contiguous run, in order.

<!-- snippet: EnumerableShouldContainInOrderExamples.ShouldContainInConsecutiveOrder.codeSample.approved.cs -->
<a id='snippet-EnumerableShouldContainInOrderExamples.ShouldContainInConsecutiveOrder.codeSample.approved.cs'></a>
```cs
var homer = new Person { Name = "Homer" };
var marge = new Person { Name = "Marge" };
var bart = new Person { Name = "Bart" };
var lisa = new Person { Name = "Lisa" };
var lineup = new List<Person> { homer, marge, bart, lisa };
lineup.ShouldContainInConsecutiveOrder([marge, lisa]);
```
<sup><a href='/src/DocumentationExamples/CodeExamples/EnumerableShouldContainInOrderExamples.ShouldContainInConsecutiveOrder.codeSample.approved.cs#L1-L6' title='Snippet source file'>snippet source</a> | <a href='#snippet-EnumerableShouldContainInOrderExamples.ShouldContainInConsecutiveOrder.codeSample.approved.cs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

**Exception**

<!-- include: EnumerableShouldContainInOrderExamples.ShouldContainInConsecutiveOrder.exceptionText.approved.txt -->
```
lineup
    should contain in consecutive order
[Marge, Lisa]
    but was actually
[Homer, Marge, Bart, Lisa]
    and the longest consecutive match was
[Marge]
    at index 1, which was not followed by
Lisa
```
<!-- endInclude -->
