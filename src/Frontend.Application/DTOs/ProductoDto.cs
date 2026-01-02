namespace Frontend.Application.DTOs;

public class ProductoDto
{
    public int IdProducto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int IdTipo { get; set; }
    public string? NombreTipo { get; set; }
    public double Precio { get; set; }
    public double Impuesto { get; set; }
}

public class ProductoListDto
{
    public List<ProductoDto> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
