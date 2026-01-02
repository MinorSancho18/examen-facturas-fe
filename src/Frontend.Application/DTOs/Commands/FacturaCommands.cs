namespace Frontend.Application.DTOs.Commands;

public class CrearFacturaCommand
{
    public int IdCliente { get; set; }
    public string NumeroFactura { get; set; } = string.Empty;
}

public class ActualizarFacturaCommand
{
    public int IdFactura { get; set; }
    public int IdCliente { get; set; }
    public string NumeroFactura { get; set; } = string.Empty;
}

public class AnularFacturaCommand
{
    public int IdFactura { get; set; }
    public string Motivo { get; set; } = string.Empty;
}
