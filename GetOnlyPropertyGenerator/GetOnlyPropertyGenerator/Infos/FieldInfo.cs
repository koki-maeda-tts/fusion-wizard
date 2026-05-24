namespace GetOnlyPropertyGenerator.Infos;

internal sealed record FieldInfo(
    string Name,
    string GeneratedPropertyName,
    string TypeFullName,
    string PropertySummary,
    LocationInfo? Location
)
{
    public string Name { get; } = Name;
    public string GeneratedPropertyName { get; } = GeneratedPropertyName;
    public string TypeFullName { get; } = TypeFullName;
    public string PropertySummary { get; } = PropertySummary;
    public LocationInfo? Location { get; } = Location;
}
