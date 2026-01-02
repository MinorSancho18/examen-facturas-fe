let clientes = [];
let productos = [];

$(document).ready(function() {
    if (esBorrador) {
        cargarClientes();
        cargarProductos();
    }
    inicializarEventos();
});

async function cargarClientes() {
    const result = await ajaxHelper.request('/Facturas/ObtenerClientes', 'GET');
    
    if (result.success && result.data.success) {
        clientes = result.data.data?.items || [];
        const select = $('#editarFacturaCliente');
        select.empty().append('<option value="">Seleccione un cliente...</option>');
        clientes.filter(c => c.activo).forEach(cliente => {
            select.append(`<option value="${cliente.idCliente}">${cliente.nombre}</option>`);
        });
        select.val(idCliente);
    }
}

async function cargarProductos() {
    const result = await ajaxHelper.request('/Facturas/ObtenerProductos', 'GET');
    
    if (result.success && result.data.success) {
        productos = result.data.data?.items || [];
        const select = $('#lineaProducto');
        select.empty().append('<option value="">Seleccione un producto...</option>');
        productos.forEach(producto => {
            select.append(`<option value="${producto.idProducto}">${producto.nombre} - $${parseFloat(producto.precio).toFixed(2)}</option>`);
        });
    }
}

function inicializarEventos() {
    if (esBorrador) {
        $('#btnAgregarLinea').click(function() {
            $('#lineaProducto').val('');
            $('#lineaCantidad').val(1);
            $('#modalAgregarLinea').modal('show');
        });

        $('#btnGuardarLinea').click(agregarLinea);

        $('.btnEditarLinea').click(function() {
            const id = $(this).data('id');
            const cantidad = $(this).data('cantidad');
            $('#editarLineaId').val(id);
            $('#editarLineaCantidad').val(cantidad);
            $('#modalEditarLinea').modal('show');
        });

        $('#btnActualizarLinea').click(actualizarLinea);

        $('.btnEliminarLinea').click(function() {
            const id = $(this).data('id');
            eliminarLinea(id);
        });

        $('#btnEditarFactura').click(function() {
            $('#editarFacturaNumero').val(numeroFactura);
            $('#modalEditarFactura').modal('show');
        });

        $('#btnGuardarEdicionFactura').click(guardarEdicionFactura);

        $('#btnEmitir').click(emitirFactura);
    }

    if (esBorrador || !esAnulada) {
        $('#btnAnular').click(function() {
            $('#motivoAnulacion').val('');
            $('#modalAnularFactura').modal('show');
        });

        $('#btnConfirmarAnular').click(anularFactura);
    }
}

async function agregarLinea() {
    const idProducto = $('#lineaProducto').val();
    const cantidad = $('#lineaCantidad').val();
    
    if (!idProducto || !cantidad || cantidad < 1) {
        ajaxHelper.showError('Por favor complete todos los campos correctamente');
        return;
    }

    const data = {
        idFactura: facturaId,
        idProducto: parseInt(idProducto),
        cantidad: parseInt(cantidad)
    };
    
    const result = await ajaxHelper.request('/Facturas/AgregarLinea', 'POST', data);
    
    if (!result.success || !result.data.success) {
        ajaxHelper.showError(result.error || result.data?.error || 'Error al agregar la línea', result.data?.errors || []);
        return;
    }
    
    ajaxHelper.showSuccess('Línea agregada exitosamente');
    $('#modalAgregarLinea').modal('hide');
    location.reload();
}

async function actualizarLinea() {
    const id = $('#editarLineaId').val();
    const cantidad = $('#editarLineaCantidad').val();
    
    if (!cantidad || cantidad < 1) {
        ajaxHelper.showError('La cantidad debe ser mayor a 0');
        return;
    }

    const data = {
        idLinea: parseInt(id),
        cantidad: parseInt(cantidad)
    };
    
    const result = await ajaxHelper.request(`/Facturas/ActualizarLinea?id=${id}`, 'PUT', data);
    
    if (!result.success || !result.data.success) {
        ajaxHelper.showError(result.error || result.data?.error || 'Error al actualizar la línea', result.data?.errors || []);
        return;
    }
    
    ajaxHelper.showSuccess('Línea actualizada exitosamente');
    $('#modalEditarLinea').modal('hide');
    location.reload();
}

async function eliminarLinea(id) {
    const confirmResult = await ajaxHelper.showConfirm(
        '¿Está seguro?',
        'Esta acción eliminará la línea de la factura',
        'Sí, eliminar',
        'Cancelar'
    );
    
    if (!confirmResult.isConfirmed) {
        return;
    }
    
    const result = await ajaxHelper.request(`/Facturas/EliminarLinea?id=${id}`, 'DELETE');
    
    if (!result.success || !result.data.success) {
        ajaxHelper.showError(result.error || result.data?.error || 'Error al eliminar la línea', result.data?.errors || []);
        return;
    }
    
    ajaxHelper.showSuccess('Línea eliminada exitosamente');
    location.reload();
}

async function guardarEdicionFactura() {
    const idClienteEdit = $('#editarFacturaCliente').val();
    const numeroEdit = $('#editarFacturaNumero').val();
    
    if (!idClienteEdit || !numeroEdit) {
        ajaxHelper.showError('Por favor complete todos los campos');
        return;
    }

    const data = {
        idFactura: facturaId,
        idCliente: parseInt(idClienteEdit),
        numeroFactura: numeroEdit
    };
    
    const result = await ajaxHelper.request(`/Facturas/Actualizar?id=${facturaId}`, 'PUT', data);
    
    if (!result.success || !result.data.success) {
        ajaxHelper.showError(result.error || result.data?.error || 'Error al actualizar la factura', result.data?.errors || []);
        return;
    }
    
    ajaxHelper.showSuccess('Factura actualizada exitosamente');
    $('#modalEditarFactura').modal('hide');
    location.reload();
}

async function emitirFactura() {
    const confirmResult = await ajaxHelper.showConfirm(
        '¿Está seguro?',
        'Esta acción emitirá la factura y no podrá ser modificada',
        'Sí, emitir',
        'Cancelar'
    );
    
    if (!confirmResult.isConfirmed) {
        return;
    }
    
    const result = await ajaxHelper.request(`/Facturas/Emitir?id=${facturaId}`, 'POST', {});
    
    if (!result.success || !result.data.success) {
        ajaxHelper.showError(result.error || result.data?.error || 'Error al emitir la factura', result.data?.errors || []);
        return;
    }
    
    ajaxHelper.showSuccess('Factura emitida exitosamente');
    location.reload();
}

async function anularFactura() {
    const motivo = $('#motivoAnulacion').val();
    
    if (!motivo) {
        ajaxHelper.showError('Por favor ingrese el motivo de anulación');
        return;
    }
    
    const data = {
        idFactura: facturaId,
        motivo: motivo
    };
    
    const result = await ajaxHelper.request(`/Facturas/Anular?id=${facturaId}`, 'POST', data);
    
    if (!result.success || !result.data.success) {
        ajaxHelper.showError(result.error || result.data?.error || 'Error al anular la factura', result.data?.errors || []);
        return;
    }
    
    ajaxHelper.showSuccess('Factura anulada exitosamente');
    $('#modalAnularFactura').modal('hide');
    location.reload();
}
