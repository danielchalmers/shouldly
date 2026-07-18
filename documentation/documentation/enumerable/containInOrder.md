# ContainInOrder

## ShouldContainInOrder

Asserts that the expected values appear in the enumerable in the same relative order. The matched values do not need to be adjacent.

<!-- snippet: EnumerableShouldContainInOrderExamples.ShouldContainInOrder.codeSample.approved.cs -->
<a id='snippet-EnumerableShouldContainInOrderExamples.ShouldContainInOrder.codeSample.approved.cs'></a>
```cs
var homer = new Person { Name = "Homer" };
var moe = new Person { Name = "Moe" };
var barney = new Person { Name = "Barney" };
var arrivals = new List<Person> { moe, homer, barney };
arrivals.ShouldContainInOrder(new List<Person> { homer, moe });
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
lineup.ShouldContainInConsecutiveOrder(new List<Person> { marge, lisa });
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
```
<!-- endInclude -->
