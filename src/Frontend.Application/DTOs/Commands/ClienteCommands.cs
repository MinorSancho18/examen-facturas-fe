namespace Frontend.Application.DTOs.Commands;

public class CrearClienteCommand
{
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
}

public class ActualizarClienteCommand
{
    public int IdCliente { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public bool Activo { get; set; }
}
