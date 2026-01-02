using Frontend.Application.DTOs.Commands;
using Frontend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Web.Controllers;

public class FacturasController : Controller
{
    private readonly IFacturasApiService _facturasService;
    private readonly ILineasFacturaApiService _lineasService;
    private readonly IClientesApiService _clientesService;
    private readonly IProductosApiService _productosService;
    private readonly IEstadosFacturaApiService _estadosService;

    public FacturasController(
        IFacturasApiService facturasService,
        ILineasFacturaApiService lineasService,
        IClientesApiService clientesService,
        IProductosApiService productosService,
        IEstadosFacturaApiService estadosService)
    {
        _facturasService = facturasService;
        _lineasService = lineasService;
        _clientesService = clientesService;
        _productosService = productosService;
        _estadosService = estadosService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var result = await _facturasService.ObtenerPorIdAsync(id, true);
        if (!result.Success || result.Data == null)
        {
            return RedirectToAction("Index");
        }
        return View(result.Data);
    }

    [HttpGet]
    public async Task<IActionResult> Listar(int? idCliente = null, DateTime? desde = null, DateTime? hasta = null, int? idEstadoFactura = null, int page = 1, int pageSize = 10)
    {
        var result = await _facturasService.ListarAsync(idCliente, desde, hasta, idEstadoFactura, page, pageSize);
        return Json(result);
    }

    [HttpGet]
    public async Task<IActionResult> Obtener(int id)
    {
        var result = await _facturasService.ObtenerPorIdAsync(id, true);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearFacturaCommand command)
    {
        var result = await _facturasService.CrearAsync(command);
        return Json(result);
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarFacturaCommand command)
    {
        command.IdFactura = id;
        var result = await _facturasService.ActualizarAsync(id, command);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> Emitir(int id)
    {
        var result = await _facturasService.EmitirAsync(id);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> Anular(int id, [FromBody] AnularFacturaCommand command)
    {
        command.IdFactura = id;
        var result = await _facturasService.AnularAsync(id, command);
        return Json(result);
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerClientes()
    {
        var result = await _clientesService.ListarAsync(1, 1000);
        return Json(result);
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerProductos()
    {
        var result = await _productosService.ListarAsync(null, 1, 1000);
        return Json(result);
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerEstados()
    {
        var result = await _estadosService.ListarAsync();
        return Json(result);
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerLineas(int idFactura)
    {
        var result = await _lineasService.ObtenerPorFacturaAsync(idFactura);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> AgregarLinea([FromBody] AgregarLineaFacturaCommand command)
    {
        var result = await _lineasService.AgregarAsync(command);
        return Json(result);
    }

    [HttpPut]
    public async Task<IActionResult> ActualizarLinea(int id, [FromBody] ActualizarLineaFacturaCommand command)
    {
        command.IdLinea = id;
        var result = await _lineasService.ActualizarAsync(id, command);
        return Json(result);
    }

    [HttpDelete]
    public async Task<IActionResult> EliminarLinea(int id)
    {
        var result = await _lineasService.EliminarAsync(id);
        return Json(result);
    }
}
