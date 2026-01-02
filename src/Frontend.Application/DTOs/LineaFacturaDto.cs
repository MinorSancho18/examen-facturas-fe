namespace Frontend.Application.DTOs;

public class LineaFacturaDto
{
    public int IdLinea { get; set; }
    public int IdFactura { get; set; }
    public int IdProducto { get; set; }
    public string? NombreProducto { get; set; }
    public int Cantidad { get; set; }
    public double PrecioUnitario { get; set; }
    public double Impuesto { get; set; }
    public double Subtotal { get; set; }
}
