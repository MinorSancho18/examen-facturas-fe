using Frontend.Application.Common;
using Frontend.Application.DTOs;
using Frontend.Application.DTOs.Commands;
using Frontend.Application.Interfaces;
using Frontend.Infrastructure.Http;

namespace Frontend.Infrastructure.Services;

public class ClientesApiService : IClientesApiService
{
    private readonly HttpJsonClient _client;

    public ClientesApiService(HttpJsonClient client)
    {
        _client = client;
    }

    public async Task<ApiResult<ClienteListDto>> ListarAsync(int page = 1, int pageSize = 10, CancellationToken ct = default)
    {
        return await _client.GetAsync<ClienteListDto>($"/api/clientes?page={page}&pageSize={pageSize}", ct);
    }

    public async Task<ApiResult<ClienteDto>> ObtenerPorIdAsync(int idCliente, CancellationToken ct = default)
    {
        return await _client.GetAsync<ClienteDto>($"/api/clientes/{idCliente}", ct);
    }

    public async Task<ApiResult<int>> CrearAsync(CrearClienteCommand command, CancellationToken ct = default)
    {
        return await _client.PostAsync<int>("/api/clientes", command, ct);
    }

    public async Task<ApiResult<bool>> ActualizarAsync(int idCliente, ActualizarClienteCommand command, CancellationToken ct = default)
    {
        var result = await _client.PutAsync<object>($"/api/clientes/{idCliente}", command, ct);
        return new ApiResult<bool>
        {
            Success = result.Success,
            StatusCode = result.StatusCode,
            Data = result.Success,
            Error = result.Error,
            Errors = result.Errors
        };
    }

    public async Task<ApiResult<bool>> EliminarAsync(int idCliente, CancellationToken ct = default)
    {
        var result = await _client.DeleteAsync<object>($"/api/clientes/{idCliente}", ct);
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
