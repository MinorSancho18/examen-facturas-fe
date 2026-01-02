using Frontend.Application.Common;
using Frontend.Application.DTOs;
using Frontend.Application.DTOs.Commands;

namespace Frontend.Application.Interfaces;

public interface IProductosApiService
{
    Task<ApiResult<ProductoListDto>> ListarAsync(int? idTipo = null, int page = 1, int pageSize = 10, CancellationToken ct = default);
    Task<ApiResult<ProductoDto>> ObtenerPorIdAsync(int idProducto, CancellationToken ct = default);
    Task<ApiResult<int>> CrearAsync(CrearProductoCommand command, CancellationToken ct = default);
    Task<ApiResult<bool>> ActualizarAsync(int idProducto, ActualizarProductoCommand command, CancellationToken ct = default);
    Task<ApiResult<bool>> EliminarAsync(int idProducto, CancellationToken ct = default);
}
