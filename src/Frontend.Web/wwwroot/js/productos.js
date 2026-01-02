let tabla;
let modoEdicion = false;
let tipos = [];

$(document).ready(function() {
    cargarTipos();
    inicializarTabla();
    inicializarEventos();
});

async function cargarTipos() {
    const result = await ajaxHelper.request('/Productos/ObtenerTipos', 'GET');
    
    if (result.success && result.data.success) {
        tipos = result.data.data || [];
        const select = $('#productoTipo');
        select.empty().append('<option value="">Seleccione un tipo...</option>');
        tipos.forEach(tipo => {
            select.append(`<option value="${tipo.idTipo}">${tipo.nombre}</option>`);
        });
    }
}

function inicializarTabla() {
    tabla = $('#tablaProductos').DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: '/Productos/Listar',
            type: 'GET',
            data: function(d) {
                return {
                    page: (d.start / d.length) + 1,
                    pageSize: d.length
                };
            },
            dataSrc: function(json) {
                if (!json.success || !json.data) {
                    return [];
                }
                return json.data.items || [];
            }
        },
        columns: [
            { data: 'idProducto' },
            { data: 'nombre' },
            { 
                data: 'nombreTipo',
                render: function(data) {
                    return data || 'N/A';
                }
            },
            { 
                data: 'precio',
                render: function(data) {
                    return '$' + parseFloat(data).toFixed(2);
                }
            },
            { 
                data: 'impuesto',
                render: function(data) {
                    return parseFloat(data).toFixed(2) + '%';
                }
            },
            {
                data: null,
                orderable: false,
                render: function(data, type, row) {
                    return `
                        <button class="btn btn-sm btn-info btnVer" data-id="${row.idProducto}">Ver</button>
                        <button class="btn btn-sm btn-warning btnEditar" data-id="${row.idProducto}">Editar</button>
                        <button class="btn btn-sm btn-danger btnEliminar" data-id="${row.idProducto}">Eliminar</button>
                    `;
                }
            }
        ],
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json'
        }
    });
}

function inicializarEventos() {
    $('#btnNuevoProducto').click(function() {
        modoEdicion = false;
        limpiarFormulario();
        $('#modalProductoTitulo').text('Nuevo Producto');
        $('#modalProducto').modal('show');
    });

    $('#btnGuardarProducto').click(guardarProducto);

    $('#tablaProductos').on('click', '.btnVer', function() {
        const id = $(this).data('id');
        verProducto(id);
    });

    $('#tablaProductos').on('click', '.btnEditar', function() {
        const id = $(this).data('id');
        editarProducto(id);
    });

    $('#tablaProductos').on('click', '.btnEliminar', function() {
        const id = $(this).data('id');
        eliminarProducto(id);
    });
}

function limpiarFormulario() {
    $('#productoId').val('');
    $('#productoNombre').val('');
    $('#productoTipo').val('');
    $('#productoPrecio').val('');
    $('#productoImpuesto').val('');
}

async function verProducto(id) {
    const result = await ajaxHelper.request(`/Productos/Obtener?id=${id}`, 'GET');
    
    if (!result.success || !result.data.success) {
        ajaxHelper.showError(result.error || result.data?.error || 'Error al obtener el producto');
        return;
    }

    const producto = result.data.data;
    $('#verProductoId').text(producto.idProducto);
    $('#verProductoNombre').text(producto.nombre);
    $('#verProductoTipo').text(producto.nombreTipo || 'N/A');
    $('#verProductoPrecio').text('$' + parseFloat(producto.precio).toFixed(2));
    $('#verProductoImpuesto').text(parseFloat(producto.impuesto).toFixed(2) + '%');
    $('#modalVerProducto').modal('show');
}

async function editarProducto(id) {
    const result = await ajaxHelper.request(`/Productos/Obtener?id=${id}`, 'GET');
    
    if (!result.success || !result.data.success) {
        ajaxHelper.showError(result.error || result.data?.error || 'Error al obtener el producto');
        return;
    }

    const producto = result.data.data;
    modoEdicion = true;
    $('#productoId').val(producto.idProducto);
    $('#productoNombre').val(producto.nombre);
    $('#productoTipo').val(producto.idTipo);
    $('#productoPrecio').val(producto.precio);
    $('#productoImpuesto').val(producto.impuesto);
    $('#modalProductoTitulo').text('Editar Producto');
    $('#modalProducto').modal('show');
}

async function guardarProducto() {
    const nombre = $('#productoNombre').val();
    const idTipo = $('#productoTipo').val();
    const precio = $('#productoPrecio').val();
    const impuesto = $('#productoImpuesto').val();
    
    if (!nombre || !idTipo || !precio || !impuesto) {
        ajaxHelper.showError('Por favor complete todos los campos');
        return;
    }

    const data = {
        nombre: nombre,
        idTipo: parseInt(idTipo),
        precio: parseFloat(precio),
        impuesto: parseFloat(impuesto)
    };

    if (modoEdicion) {
        const id = $('#productoId').val();
        data.idProducto = parseInt(id);
        
        const result = await ajaxHelper.request(`/Productos/Actualizar?id=${id}`, 'PUT', data);
        
        if (!result.success || !result.data.success) {
            ajaxHelper.showError(result.error || result.data?.error || 'Error al actualizar el producto', result.data?.errors || []);
            return;
        }
        
        ajaxHelper.showSuccess('Producto actualizado exitosamente');
    } else {
        const result = await ajaxHelper.request('/Productos/Crear', 'POST', data);
        
        if (!result.success || !result.data.success) {
            ajaxHelper.showError(result.error || result.data?.error || 'Error al crear el producto', result.data?.errors || []);
            return;
        }
        
        ajaxHelper.showSuccess('Producto creado exitosamente');
    }
    
    $('#modalProducto').modal('hide');
    tabla.ajax.reload();
}

async function eliminarProducto(id) {
    const confirmResult = await ajaxHelper.showConfirm(
        '¿Está seguro?',
        'Esta acción eliminará el producto permanentemente',
        'Sí, eliminar',
        'Cancelar'
    );
    
    if (!confirmResult.isConfirmed) {
        return;
    }
    
    const result = await ajaxHelper.request(`/Productos/Eliminar?id=${id}`, 'DELETE');
    
    if (!result.success || !result.data.success) {
        ajaxHelper.showError(result.error || result.data?.error || 'Error al eliminar el producto', result.data?.errors || []);
        return;
    }
    
    ajaxHelper.showSuccess('Producto eliminado exitosamente');
    tabla.ajax.reload();
}
