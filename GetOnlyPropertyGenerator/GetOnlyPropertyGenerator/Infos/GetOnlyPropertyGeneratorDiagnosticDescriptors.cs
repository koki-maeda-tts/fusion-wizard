using Microsoft.CodeAnalysis;

namespace GetOnlyPropertyGenerator.Infos;

internal static class GetOnlyPropertyGeneratorDiagnosticDescriptors
{
    public const string Id = "GetOnlyPropertyGenerator";
    public const string Category = "GetOnlyPropertyGenerator";

    public static readonly DiagnosticDescriptor MustBePartial = new(
        id: Id + "001",
        title: "Must be partial",
        messageFormat: "Type '{0}', which contains a field with the GenerateGetOnlyProperty attribute, must be marked as partial",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor AlreadyExist = new(
        id: Id + "002",
        title: "Property already exists",
        messageFormat: """
        A property or field with the same name as the generated property '{0}' already exists
        """,
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor DuplicateGeneration = new(
        id: Id + "003",
        title: "Duplicate generation",
        messageFormat: """
        There is a duplicate name '{0}' among the automatically generated properties
        """,
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor PublicOrInternalField = new(
        id: Id + "004",
        title: "Public or Internal field",
        messageFormat: "The access modifier is set to `public` or `internal`",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor MustNotBeNested = new(
        id: Id + "005",
        title: "Must not be nested",
        messageFormat: "Type '{0}', which uses GenerateGetOnlyProperty, must not be nested",
        category: Category,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
}
