using Frontend.Application.Common;
using Frontend.Application.DTOs;
using Frontend.Application.DTOs.Commands;

namespace Frontend.Application.Interfaces;

public interface IFacturasApiService
{
    Task<ApiResult<FacturaListDto>> ListarAsync(int? idCliente = null, DateTime? desde = null, DateTime? hasta = null, int? idEstadoFactura = null, int page = 1, int pageSize = 10, CancellationToken ct = default);
    Task<ApiResult<FacturaDto>> ObtenerPorIdAsync(int idFactura, bool incluirDetalle = true, CancellationToken ct = default);
    Task<ApiResult<int>> CrearAsync(CrearFacturaCommand command, CancellationToken ct = default);
    Task<ApiResult<bool>> ActualizarAsync(int idFactura, ActualizarFacturaCommand command, CancellationToken ct = default);
    Task<ApiResult<bool>> EmitirAsync(int idFactura, CancellationToken ct = default);
    Task<ApiResult<bool>> AnularAsync(int idFactura, AnularFacturaCommand command, CancellationToken ct = default);
}
