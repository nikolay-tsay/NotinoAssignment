using Newtonsoft.Json;

namespace NotinoAssignment.Middleware;

public sealed class ErrorResponse
{
    public required int StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
    public Exception? InnerException { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}