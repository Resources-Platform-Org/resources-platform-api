namespace Api.Wrappers;

public class ApiResponse<T>
{
    public bool Succeeded { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }
    public T? Data { get; set; }

    // Constructor for Successful Response with Data
    public ApiResponse(T data, string? message = null)
    {
        Succeeded = true;
        Message = message;
        Data = data;
        Errors = null;
    }

    // Constructor for Failed Response with Errors
    public ApiResponse(string message, List<string> errors)
    {
        Succeeded = false;
        Message = message;
        Errors = errors;
        Data = default;
    }
}