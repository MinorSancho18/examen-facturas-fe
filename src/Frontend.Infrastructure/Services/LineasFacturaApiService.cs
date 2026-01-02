using Frontend.Application.Common;
using Frontend.Application.DTOs;
using Frontend.Application.DTOs.Commands;
using Frontend.Application.Interfaces;
using Frontend.Infrastructure.Http;

namespace Frontend.Infrastructure.Services;

public class LineasFacturaApiService : ILineasFacturaApiService
{
    private readonly HttpJsonClient _client;

    public LineasFacturaApiService(HttpJsonClient client)
    {
        _client = client;
    }

    public async Task<ApiResult<List<LineaFacturaDto>>> ObtenerPorFacturaAsync(int idFactura, CancellationToken ct = default)
    {
        return await _client.GetAsync<List<LineaFacturaDto>>($"/api/lineas-factura/por-factura/{idFactura}", ct);
    }

    public async Task<ApiResult<int>> AgregarAsync(AgregarLineaFacturaCommand command, CancellationToken ct = default)
    {
        return await _client.PostAsync<int>("/api/lineas-factura", command, ct);
    }

    public async Task<ApiResult<bool>> ActualizarAsync(int idLinea, ActualizarLineaFacturaCommand command, CancellationToken ct = default)
    {
        var result = await _client.PutAsync<object>($"/api/lineas-factura/{idLinea}", command, ct);
        return new ApiResult<bool>
        {
            Success = result.Success,
            StatusCode = result.StatusCode,
            Data = result.Success,
            Error = result.Error,
            Errors = result.Errors
        };
    }

    public async Task<ApiResult<bool>> EliminarAsync(int idLinea, CancellationToken ct = default)
    {
        var result = await _client.DeleteAsync<object>($"/api/lineas-factura/{idLinea}", ct);
        return new ApiResult<bool>
        {
            Success = result.Success,
            StatusCode = result.StatusCode,
            Data = result.Success,
            Error = result.Error,
            Errors = result.Errors
        };
    }
}
