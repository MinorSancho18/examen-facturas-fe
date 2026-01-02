using Frontend.Application.Common;
using Frontend.Application.DTOs;
using Frontend.Application.DTOs.Commands;
using Frontend.Application.Interfaces;
using Frontend.Infrastructure.Http;

namespace Frontend.Infrastructure.Services;

public class FacturasApiService : IFacturasApiService
{
    private readonly HttpJsonClient _client;

    public FacturasApiService(HttpJsonClient client)
    {
        _client = client;
    }

    public async Task<ApiResult<FacturaListDto>> ListarAsync(int? idCliente = null, DateTime? desde = null, DateTime? hasta = null, int? idEstadoFactura = null, int page = 1, int pageSize = 10, CancellationToken ct = default)
    {
        var url = $"/api/facturas?page={page}&pageSize={pageSize}";
        
        if (idCliente.HasValue)
            url += $"&idCliente={idCliente.Value}";
        if (desde.HasValue)
            url += $"&desde={desde.Value:yyyy-MM-dd}";
        if (hasta.HasValue)
            url += $"&hasta={hasta.Value:yyyy-MM-dd}";
        if (idEstadoFactura.HasValue)
            url += $"&idEstadoFactura={idEstadoFactura.Value}";
        
        return await _client.GetAsync<FacturaListDto>(url, ct);
    }

    public async Task<ApiResult<FacturaDto>> ObtenerPorIdAsync(int idFactura, bool incluirDetalle = true, CancellationToken ct = default)
    {
        return await _client.GetAsync<FacturaDto>($"/api/facturas/{idFactura}?incluirDetalle={incluirDetalle}", ct);
    }

    public async Task<ApiResult<int>> CrearAsync(CrearFacturaCommand command, CancellationToken ct = default)
    {
        return await _client.PostAsync<int>("/api/facturas", command, ct);
    }

    public async Task<ApiResult<bool>> ActualizarAsync(int idFactura, ActualizarFacturaCommand command, CancellationToken ct = default)
    {
        var result = await _client.PutAsync<object>($"/api/facturas/{idFactura}", command, ct);
        return new ApiResult<bool>
        {
            Success = result.Success,
            StatusCode = result.StatusCode,
            Data = result.Success,
            Error = result.Error,
            Errors = result.Errors
        };
    }

    public async Task<ApiResult<bool>> EmitirAsync(int idFactura, CancellationToken ct = default)
    {
        var result = await _client.PostAsync<object>($"/api/facturas/{idFactura}/emitir", new { }, ct);
        return new ApiResult<bool>
        {
            Success = result.Success,
            StatusCode = result.StatusCode,
            Data = result.Success,
            Error = result.Error,
            Errors = result.Errors
        };
    }

    public async Task<ApiResult<bool>> AnularAsync(int idFactura, AnularFacturaCommand command, CancellationToken ct = default)
    {
        var result = await _client.PostAsync<object>($"/api/facturas/{idFactura}/anular", command, ct);
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
