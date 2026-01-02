namespace Frontend.Application.DTOs;

public class FacturaDto
{
    public int IdFactura { get; set; }
    public int IdCliente { get; set; }
    public string? NombreCliente { get; set; }
    public string NumeroFactura { get; set; } = string.Empty;
    public DateTime FechaEmision { get; set; }
    public int IdEstadoFactura { get; set; }
    public string? NombreEstado { get; set; }
    public double Total { get; set; }
    public string? MotivoAnulacion { get; set; }
    public List<LineaFacturaDto> Lineas { get; set; } = new();
}

public class FacturaListDto
{
    public List<FacturaDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
