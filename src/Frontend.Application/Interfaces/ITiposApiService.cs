using Frontend.Application.Common;
using Frontend.Application.DTOs;

namespace Frontend.Application.Interfaces;

public interface ITiposApiService
{
    Task<ApiResult<List<TipoDto>>> ListarAsync(CancellationToken ct = default);
}
