namespace Frontend.Application.DTOs.Commands;

public class AgregarLineaFacturaCommand
{
    public int IdFactura { get; set; }
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
}

public class ActualizarLineaFacturaCommand
{
    public int IdLinea { get; set; }
    public int Cantidad { get; set; }
}
