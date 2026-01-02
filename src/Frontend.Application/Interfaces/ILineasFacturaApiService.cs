using Frontend.Application.Common;
using Frontend.Application.DTOs;
using Frontend.Application.DTOs.Commands;

namespace Frontend.Application.Interfaces;

public interface ILineasFacturaApiService
{
    Task<ApiResult<List<LineaFacturaDto>>> ObtenerPorFacturaAsync(int idFactura, CancellationToken ct = default);
    Task<ApiResult<int>> AgregarAsync(AgregarLineaFacturaCommand command, CancellationToken ct = default);
    Task<ApiResult<bool>> ActualizarAsync(int idLinea, ActualizarLineaFacturaCommand command, CancellationToken ct = default);
    Task<ApiResult<bool>> EliminarAsync(int idLinea, CancellationToken ct = default);
}
