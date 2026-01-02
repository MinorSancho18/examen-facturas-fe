using Frontend.Application.DTOs.Commands;
using Frontend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Web.Controllers;

public class ProductosController : Controller
{
    private readonly IProductosApiService _productosService;
    private readonly ITiposApiService _tiposService;

    public ProductosController(IProductosApiService productosService, ITiposApiService tiposService)
    {
        _productosService = productosService;
        _tiposService = tiposService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Listar(int? idTipo = null, int page = 1, int pageSize = 10)
    {
        var result = await _productosService.ListarAsync(idTipo, page, pageSize);
        return Json(result);
    }

    [HttpGet]
    public async Task<IActionResult> Obtener(int id)
    {
        var result = await _productosService.ObtenerPorIdAsync(id);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearProductoCommand command)
    {
        var result = await _productosService.CrearAsync(command);
        return Json(result);
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarProductoCommand command)
    {
        command.IdProducto = id;
        var result = await _productosService.ActualizarAsync(id, command);
        return Json(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Eliminar(int id)
    {
        var result = await _productosService.EliminarAsync(id);
        return Json(result);
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTipos()
    {
        var result = await _tiposService.ListarAsync();
        return Json(result);
    }
}
