namespace Shouldly;

/// <summary>
/// Options controlling how <c>ShouldBeEquivalentTo</c> compares object graphs. Aligns with the in-flight
/// `#1094` options type and addresses `#1075`.
/// </summary>
public class EquivalencyOptions
{
    /// <summary>
    /// When true, top-level collections are compared as multisets (element order is ignored). Defaults to
    /// false, preserving Shouldly's order-sensitive comparison.
    /// </summary>
    public bool IgnoreOrder { get; set; }
}
