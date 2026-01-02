using Frontend.Application.Common;
using Frontend.Application.DTOs;
using Frontend.Application.DTOs.Commands;
using Frontend.Application.Interfaces;
using Frontend.Infrastructure.Http;

namespace Frontend.Infrastructure.Services;

public class ProductosApiService : IProductosApiService
{
    private readonly HttpJsonClient _client;

    public ProductosApiService(HttpJsonClient client)
    {
        _client = client;
    }

    public async Task<ApiResult<ProductoListDto>> ListarAsync(int? idTipo = null, int page = 1, int pageSize = 10, CancellationToken ct = default)
    {
        var url = $"/api/productos?page={page}&pageSize={pageSize}";
        if (idTipo.HasValue)
            url += $"&idTipo={idTipo.Value}";
        
        return await _client.GetAsync<ProductoListDto>(url, ct);
    }

    public async Task<ApiResult<ProductoDto>> ObtenerPorIdAsync(int idProducto, CancellationToken ct = default)
    {
        return await _client.GetAsync<ProductoDto>($"/api/productos/{idProducto}", ct);
    }

    public async Task<ApiResult<int>> CrearAsync(CrearProductoCommand command, CancellationToken ct = default)
    {
        return await _client.PostAsync<int>("/api/productos", command, ct);
    }

    public async Task<ApiResult<bool>> ActualizarAsync(int idProducto, ActualizarProductoCommand command, CancellationToken ct = default)
    {
        var result = await _client.PutAsync<object>($"/api/productos/{idProducto}", command, ct);
        return new ApiResult<bool>
        {
            Success = result.Success,
            StatusCode = result.StatusCode,
            Data = result.Success,
            Error = result.Error,
            Errors = result.Errors
        };
    }

    public async Task<ApiResult<bool>> EliminarAsync(int idProducto, CancellationToken ct = default)
    {
        var result = await _client.DeleteAsync<object>($"/api/productos/{idProducto}", ct);
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
