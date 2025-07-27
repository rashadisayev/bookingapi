namespace BookingApp.Common.Exceptions;

public class ApplicationError : Exception
{
    public ErrorType ErrorType { get; }
    public string? Details { get; }

    public ApplicationError(ErrorType errorType, string? message = null, string? details = null)
        : base(message ?? errorType.ToString())
    {
        ErrorType = errorType;
        Details = details;
    }

    public ApplicationError(ErrorType errorType, Exception innerException, string? details = null)
        : base(errorType.ToString(), innerException)
    {
        ErrorType = errorType;
        Details = details;
    }
}