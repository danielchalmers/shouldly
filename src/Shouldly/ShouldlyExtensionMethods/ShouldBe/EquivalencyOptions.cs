namespace Shouldly;

/// <summary>
/// Options controlling how <c>ShouldBeEquivalentTo</c> compares object graphs.
/// </summary>
/// <remarks>
/// Intended to be the single extensibility point for equivalence tweaks (mirrors the direction of
/// shouldly/shouldly#1094, which introduces this type with <c>CompareUsingRuntimeTypes</c>). This prototype
/// adds <see cref="IgnoreOrder"/>; the two should merge into one options object.
/// </remarks>
public class EquivalencyOptions
{
    /// <summary>
    /// When true, top-level collections are compared as multisets (element order is ignored), matching
    /// FluentAssertions' default BeEquivalentTo behaviour. Defaults to false (Shouldly's order-sensitive default).
    /// </summary>
    public bool IgnoreOrder { get; set; }
}
