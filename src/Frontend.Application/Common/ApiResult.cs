namespace Frontend.Application.Common;

public class ApiResult<T>
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }
    public List<string> Errors { get; set; } = new();
}
