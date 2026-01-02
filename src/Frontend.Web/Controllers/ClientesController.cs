using Frontend.Application.DTOs.Commands;
using Frontend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Web.Controllers;

public class ClientesController : Controller
{
    private readonly IClientesApiService _clientesService;

    public ClientesController(IClientesApiService clientesService)
    {
        _clientesService = clientesService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Listar(int page = 1, int pageSize = 10)
    {
        var result = await _clientesService.ListarAsync(page, pageSize);
        return Json(result);
    }

    [HttpGet]
    public async Task<IActionResult> Obtener(int id)
    {
        var result = await _clientesService.ObtenerPorIdAsync(id);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearClienteCommand command)
    {
        var result = await _clientesService.CrearAsync(command);
        return Json(result);
    }

    [HttpPut]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarClienteCommand command)
    {
        command.IdCliente = id;
        var result = await _clientesService.ActualizarAsync(id, command);
        return Json(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Eliminar(int id)
    {
        var result = await _clientesService.EliminarAsync(id);
        return Json(result);
    }
}
