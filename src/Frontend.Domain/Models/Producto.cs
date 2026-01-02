namespace Frontend.Domain.Models;

public class Producto
{
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int IdTipo { get; set; }
    public double Precio { get; set; }
    public double Impuesto { get; set; }
}
