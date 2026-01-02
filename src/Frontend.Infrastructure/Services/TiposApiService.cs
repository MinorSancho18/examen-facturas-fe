using Frontend.Application.Common;
using Frontend.Application.DTOs;
using Frontend.Application.Interfaces;
using Frontend.Infrastructure.Http;

namespace Frontend.Infrastructure.Services;

public class TiposApiService : ITiposApiService
{
    private readonly HttpJsonClient _client;

    public TiposApiService(HttpJsonClient client)
    {
        _client = client;
    }

    public async Task<ApiResult<List<TipoDto>>> ListarAsync(CancellationToken ct = default)
    {
        return await _client.GetAsync<List<TipoDto>>("/api/tipos", ct);
    }
}
