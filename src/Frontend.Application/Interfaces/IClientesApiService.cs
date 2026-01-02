using Frontend.Application.Common;
using Frontend.Application.DTOs;
using Frontend.Application.DTOs.Commands;

namespace Frontend.Application.Interfaces;

public interface IClientesApiService
{
    Task<ApiResult<ClienteListDto>> ListarAsync(int page = 1, int pageSize = 10, CancellationToken ct = default);
    Task<ApiResult<ClienteDto>> ObtenerPorIdAsync(int idCliente, CancellationToken ct = default);
    Task<ApiResult<int>> CrearAsync(CrearClienteCommand command, CancellationToken ct = default);
    Task<ApiResult<bool>> ActualizarAsync(int idCliente, ActualizarClienteCommand command, CancellationToken ct = default);
    Task<ApiResult<bool>> EliminarAsync(int idCliente, CancellationToken ct = default);
}
