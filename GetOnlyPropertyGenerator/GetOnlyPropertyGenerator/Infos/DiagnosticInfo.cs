using Microsoft.CodeAnalysis;

namespace GetOnlyPropertyGenerator.Infos;

internal sealed record DiagnosticInfo
{
    public DiagnosticInfo(DiagnosticDescriptor descriptor, Location? location, string message)
    {
        Descriptor = descriptor;
        Location = location is null ? null : LocationInfo.CreateFrom(location);
        Message = message;
    }

    public DiagnosticDescriptor Descriptor { get; }
    public LocationInfo? Location { get; }
    public string Message { get; }
}
