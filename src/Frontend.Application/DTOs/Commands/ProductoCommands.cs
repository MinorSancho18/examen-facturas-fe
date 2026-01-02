namespace Frontend.Application.DTOs.Commands;

public class CrearProductoCommand
{
    public string Nombre { get; set; } = string.Empty;
    public int IdTipo { get; set; }
    public double Precio { get; set; }
    public double Impuesto { get; set; }
}

public class ActualizarProductoCommand
{
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int IdTipo { get; set; }
    public double Precio { get; set; }
    public double Impuesto { get; set; }
}
