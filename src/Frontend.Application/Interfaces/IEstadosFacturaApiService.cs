using Frontend.Application.Common;
using Frontend.Application.DTOs;

namespace Frontend.Application.Interfaces;

public interface IEstadosFacturaApiService
{
    Task<ApiResult<List<EstadoFacturaDto>>> ListarAsync(CancellationToken ct = default);
}
