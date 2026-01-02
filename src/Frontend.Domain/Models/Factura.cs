namespace Frontend.Domain.Models;

public class Factura
{
    public int IdFactura { get; set; }
    public int IdCliente { get; set; }
    public string NumeroFactura { get; set; } = string.Empty;
    public DateTime FechaEmision { get; set; }
    public int IdEstadoFactura { get; set; }
    public double Total { get; set; }
    public string? MotivoAnulacion { get; set; }
    
    // Navigation properties
    public Cliente? Cliente { get; set; }
    public EstadoFactura? EstadoFactura { get; set; }
    public List<LineaFactura> Lineas { get; set; } = new();
}
