namespace GetOnlyPropertyGenerator.Infos;

internal sealed record ContainingInfo(
    string ContainingTypeNameIncludedTypeKeyword,
    string ContainingTypeFullName,
    string ContainingNamespaceFullName,
    bool ContainingNamespaceIsGlobalNamespace
)
{
    public string ContainingTypeNameIncludedTypeKeyword { get; } =
        ContainingTypeNameIncludedTypeKeyword;
    public string ContainingTypeFullName { get; } = ContainingTypeFullName;
    public string ContainingNamespaceFullName { get; } = ContainingNamespaceFullName;
    public bool ContainingNamespaceIsGlobalNamespace { get; } =
        ContainingNamespaceIsGlobalNamespace;
}
