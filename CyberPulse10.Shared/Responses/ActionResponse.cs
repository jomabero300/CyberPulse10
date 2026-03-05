namespace CyberPulse10.Shared.Responses;

public class ActionResponse<T>
{
    public bool WasSuccess { get; init; }
    public string? Message { get; init; }
    public T? Result { get; init; }

    public static ActionResponse<T> Success(T result) =>
        new() { WasSuccess = true, Result = result };

    public static ActionResponse<T> Fail(string message) =>
        new() { WasSuccess = false, Message = message };
}