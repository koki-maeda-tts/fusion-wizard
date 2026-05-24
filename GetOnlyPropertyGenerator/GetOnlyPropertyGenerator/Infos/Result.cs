namespace GetOnlyPropertyGenerator.Infos;

internal sealed record Result<TValue>(TValue Value, EquatableArray<DiagnosticInfo> Errors)
    where TValue : IEquatable<TValue>
{
    public TValue Value { get; } = Value;
    public EquatableArray<DiagnosticInfo> Errors { get; } = Errors;
}
