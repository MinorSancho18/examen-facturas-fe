namespace Frontend.Domain.Models;

public class LineaFactura
{
    public int IdLinea { get; set; }
    public int IdFactura { get; set; }
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    public double PrecioUnitario { get; set; }
    public double Impuesto { get; set; }
    public double Subtotal { get; set; }
    
    // Navigation properties
    public Producto? Producto { get; set; }
}
