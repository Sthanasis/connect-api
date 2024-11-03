using System.Text.Json.Serialization;

namespace connect.Models;

public class ErrorViewModel
{
    [JsonPropertyName("requestId")]
    public string? RequestId { get; set; }
    [JsonPropertyName("showRequestId")]
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
