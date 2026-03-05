namespace CyberPulse10.Shared.Responses;

public sealed class PagedResult<T>
{
    public IEnumerable<T> Items { get; init; } = [];
    public int TotalRecords { get; init; }
}