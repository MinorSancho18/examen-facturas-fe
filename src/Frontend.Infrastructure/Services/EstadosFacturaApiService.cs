using Frontend.Application.Common;
using Frontend.Application.DTOs;
using Frontend.Application.Interfaces;
using Frontend.Infrastructure.Http;

namespace Frontend.Infrastructure.Services;

public class EstadosFacturaApiService : IEstadosFacturaApiService
{
    private readonly HttpJsonClient _client;

    public EstadosFacturaApiService(HttpJsonClient client)
    {
        _client = client;
    }

    public async Task<ApiResult<List<EstadoFacturaDto>>> ListarAsync(CancellationToken ct = default)
    {
        return await _client.GetAsync<List<EstadoFacturaDto>>("/api/estados-factura", ct);
    }
}
