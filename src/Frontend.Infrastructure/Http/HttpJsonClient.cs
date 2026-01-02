using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Frontend.Application.Common;

namespace Frontend.Infrastructure.Http;

public sealed class HttpJsonClient
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _http;

    public HttpJsonClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<ApiResult<T>> GetAsync<T>(string url, CancellationToken ct = default)
        => await SendAsync<T>(HttpMethod.Get, url, null, ct);

    public async Task<ApiResult<T>> PostAsync<T>(string url, object body, CancellationToken ct = default)
        => await SendAsync<T>(HttpMethod.Post, url, body, ct);

    public async Task<ApiResult<T>> PutAsync<T>(string url, object body, CancellationToken ct = default)
        => await SendAsync<T>(HttpMethod.Put, url, body, ct);

    public async Task<ApiResult<T>> DeleteAsync<T>(string url, CancellationToken ct = default)
        => await SendAsync<T>(HttpMethod.Delete, url, null, ct);

    private async Task<ApiResult<T>> SendAsync<T>(HttpMethod method, string url, object? body, CancellationToken ct)
    {
        using var req = new HttpRequestMessage(method, url);
        if (body is not null)
            req.Content = JsonContent.Create(body, options: JsonOptions);

        using var res = await _http.SendAsync(req, ct);

        var result = new ApiResult<T>
        {
            StatusCode = (int)res.StatusCode,
            Success = res.IsSuccessStatusCode
        };

        if (res.StatusCode == HttpStatusCode.NoContent)
        {
            result.Success = true;
            return result;
        }

        string? raw = null;
        try { raw = await res.Content.ReadAsStringAsync(ct); }
        catch { /* ignore */ }

        if (string.IsNullOrWhiteSpace(raw))
            return result;

        if (res.IsSuccessStatusCode)
        {
            try { result.Data = JsonSerializer.Deserialize<T>(raw, JsonOptions); }
            catch
            {
                try
                {
                    using var doc = JsonDocument.Parse(raw);
                    if (typeof(T) == typeof(int) && doc.RootElement.TryGetProperty("id", out var id))
                        result.Data = (T)(object)id.GetInt32();
                }
                catch { }
            }
            return result;
        }

        // Error parsing
        try
        {
            using var doc = JsonDocument.Parse(raw);
            var root = doc.RootElement;

            if (root.ValueKind == JsonValueKind.Object)
            {
                if (root.TryGetProperty("error", out var errProp))
                    result.Error = errProp.GetString();

                if (root.TryGetProperty("errors", out var errsProp))
                {
                    if (errsProp.ValueKind == JsonValueKind.Array)
                        result.Errors.AddRange(errsProp.EnumerateArray().Select(x => x.GetString() ?? string.Empty).Where(x => !string.IsNullOrWhiteSpace(x)));
                    else if (errsProp.ValueKind == JsonValueKind.Object)
                    {
                        foreach (var p in errsProp.EnumerateObject())
                        {
                            if (p.Value.ValueKind == JsonValueKind.Array)
                                result.Errors.AddRange(p.Value.EnumerateArray().Select(x => x.GetString() ?? string.Empty).Where(x => !string.IsNullOrWhiteSpace(x)));
                        }
                    }
                }
            }
        }
        catch { result.Error = raw; }

        if (string.IsNullOrWhiteSpace(result.Error) && result.Errors.Count == 0)
            result.Error = raw;

        return result;
    }
}
