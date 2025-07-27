namespace BookingApi.Models;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; } = true;
    public string? Message { get; set; }
    public T? Data { get; set; }

    public ApiResponse() { }
    internal ApiResponse(bool isSuccess, T? data = default(T), string? message = null)
    {
        IsSuccess = isSuccess;
        Data = data;
        Message = message;
    }

    public static ApiResponse<T> Success(T data, string? message = null)
        => new(true, data, message);

    public static ApiResponse<T> Error(string errorMessage)
        => new(false, default(T), errorMessage);
}