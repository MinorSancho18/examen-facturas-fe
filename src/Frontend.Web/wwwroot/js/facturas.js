let tabla;
let modoEdicion = false;
let clientes = [];
let estados = [];

$(document).ready(function() {
    cargarClientes();
    cargarEstados();
    inicializarTabla();
    inicializarEventos();
});

async function cargarClientes() {
    const result = await ajaxHelper.request('/Facturas/ObtenerClientes', 'GET');
    
    if (result.success && result.data.success) {
        clientes = result.data.data?.items || [];
        const select = $('.select-clientes, #facturaCliente, #filtroCliente');
        select.each(function() {
            const isFilter = $(this).attr('id') === 'filtroCliente';
            $(this).empty();
            if (isFilter) {
                $(this).append('<option value="">Todos</option>');
            } else {
                $(this).append('<option value="">Seleccione un cliente...</option>');
            }
            clientes.filter(c => c.activo).forEach(cliente => {
                $(this).append(`<option value="${cliente.idCliente}">${cliente.nombre}</option>`);
            });
        });
    }
}

async function cargarEstados() {
    const result = await ajaxHelper.request('/Facturas/ObtenerEstados', 'GET');
    
    if (result.success && result.data.success) {
        estados = result.data.data || [];
        const select = $('#filtroEstado');
        select.empty().append('<option value="">Todos</option>');
        estados.forEach(estado => {
            select.append(`<option value="${estado.idEstadoFactura}">${estado.nombre}</option>`);
        });
    }
}

function inicializarTabla() {
    tabla = $('#tablaFacturas').DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: '/Facturas/Listar',
            type: 'GET',
            data: function(d) {
                const filtroCliente = $('#filtroCliente').val();
                const filtroDesde = $('#filtroDesde').val();
                const filtroHasta = $('#filtroHasta').val();
                const filtroEstado = $('#filtroEstado').val();
                
                return {
                    page: (d.start / d.length) + 1,
                    pageSize: d.length,
                    idCliente: filtroCliente || null,
                    desde: filtroDesde || null,
                    hasta: filtroHasta || null,
                    idEstadoFactura: filtroEstado || null
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
            { data: 'idFactura' },
            { data: 'numeroFactura' },
            { data: 'nombreCliente' },
            { 
                data: 'fechaEmision',
                render: function(data) {
                    return new Date(data).toLocaleDateString('es-ES');
                }
            },
            { 
                data: 'nombreEstado',
                render: function(data, type, row) {
                    const badgeClass = row.idEstadoFactura === 1 ? 'secondary' : (row.idEstadoFactura === 2 ? 'success' : 'danger');
                    return `<span class="badge bg-${badgeClass}">${data}</span>`;
                }
            },
            { 
                data: 'total',
                render: function(data) {
                    return '$' + parseFloat(data).toFixed(2);
                }
            },
            {
                data: null,
                orderable: false,
                render: function(data, type, row) {
                    return `
                        <button class="btn btn-sm btn-info btnVer" data-id="${row.idFactura}">Ver</button>
                        <a href="/Facturas/Detalle/${row.idFactura}" class="btn btn-sm btn-primary">Detalle</a>
                        ${row.idEstadoFactura === 1 ? `<button class="btn btn-sm btn-warning btnEditar" data-id="${row.idFactura}">Editar</button>` : ''}
                        ${row.idEstadoFactura === 1 ? `<button class="btn btn-sm btn-success btnEmitir" data-id="${row.idFactura}">Emitir</button>` : ''}
                        ${row.idEstadoFactura !== 3 ? `<button class="btn btn-sm btn-danger btnAnular" data-id="${row.idFactura}">Anular</button>` : ''}
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
    $('#btnNuevaFactura').click(function() {
        modoEdicion = false;
        limpiarFormulario();
        $('#modalFacturaTitulo').text('Nueva Factura');
        $('#modalFactura').modal('show');
    });

    $('#btnGuardarFactura').click(guardarFactura);

    $('#btnFiltrar').click(function() {
        tabla.ajax.reload();
    });

    $('#btnLimpiarFiltros').click(function() {
        $('#filtroCliente').val('');
        $('#filtroDesde').val('');
        $('#filtroHasta').val('');
        $('#filtroEstado').val('');
        tabla.ajax.reload();
    });

    $('#tablaFacturas').on('click', '.btnVer', function() {
        const id = $(this).data('id');
        verFactura(id);
    });

    $('#tablaFacturas').on('click', '.btnEditar', function() {
        const id = $(this).data('id');
        editarFactura(id);
    });

    $('#tablaFacturas').on('click', '.btnEmitir', function() {
        const id = $(this).data('id');
        emitirFactura(id);
    });

    $('#tablaFacturas').on('click', '.btnAnular', function() {
        const id = $(this).data('id');
        mostrarModalAnular(id);
    });

    $('#btnConfirmarAnular').click(confirmarAnular);
}

function limpiarFormulario() {
    $('#facturaId').val('');
    $('#facturaCliente').val('');
    $('#facturaNumero').val('');
}

async function verFactura(id) {
    const result = await ajaxHelper.request(`/Facturas/Obtener?id=${id}`, 'GET');
    
    if (!result.success || !result.data.success) {
        ajaxHelper.showError(result.error || result.data?.error || 'Error al obtener la factura');
        return;
    }

    const factura = result.data.data;
    const estadoBadge = factura.idEstadoFactura === 1 ? 'secondary' : (factura.idEstadoFactura === 2 ? 'success' : 'danger');
    
    $('#verFacturaId').text(factura.idFactura);
    $('#verFacturaNumero').text(factura.numeroFactura);
    $('#verFacturaCliente').text(factura.nombreCliente);
    $('#verFacturaFecha').text(new Date(factura.fechaEmision).toLocaleDateString('es-ES'));
    $('#verFacturaEstado').html(`<span class="badge bg-${estadoBadge}">${factura.nombreEstado}</span>`);
    $('#verFacturaTotal').text('$' + parseFloat(factura.total).toFixed(2));
    $('#btnVerDetalle').attr('href', `/Facturas/Detalle/${factura.idFactura}`);
    $('#modalVerFactura').modal('show');
}

async function editarFactura(id) {
    const result = await ajaxHelper.request(`/Facturas/Obtener?id=${id}`, 'GET');
    
    if (!result.success || !result.data.success) {
        ajaxHelper.showError(result.error || result.data?.error || 'Error al obtener la factura');
        return;
    }

    const factura = result.data.data;
    
    if (factura.idEstadoFactura !== 1) {
        ajaxHelper.showError('Solo se pueden editar facturas en estado Borrador');
        return;
    }
    
    modoEdicion = true;
    $('#facturaId').val(factura.idFactura);
    $('#facturaCliente').val(factura.idCliente);
    $('#facturaNumero').val(factura.numeroFactura);
    $('#modalFacturaTitulo').text('Editar Factura');
    $('#modalFactura').modal('show');
}

async function guardarFactura() {
    const idCliente = $('#facturaCliente').val();
    const numeroFactura = $('#facturaNumero').val();
    
    if (!idCliente || !numeroFactura) {
        ajaxHelper.showError('Por favor complete todos los campos');
        return;
    }

    const data = {
        idCliente: parseInt(idCliente),
        numeroFactura: numeroFactura
    };

    if (modoEdicion) {
        const id = $('#facturaId').val();
        data.idFactura = parseInt(id);
        
        const result = await ajaxHelper.request(`/Facturas/Actualizar?id=${id}`, 'PUT', data);
        
        if (!result.success || !result.data.success) {
            ajaxHelper.showError(result.error || result.data?.error || 'Error al actualizar la factura', result.data?.errors || []);
            return;
        }
        
        ajaxHelper.showSuccess('Factura actualizada exitosamente');
    } else {
        const result = await ajaxHelper.request('/Facturas/Crear', 'POST', data);
        
        if (!result.success || !result.data.success) {
            ajaxHelper.showError(result.error || result.data?.error || 'Error al crear la factura', result.data?.errors || []);
            return;
        }
        
        ajaxHelper.showSuccess('Factura creada exitosamente');
    }
    
    $('#modalFactura').modal('hide');
    tabla.ajax.reload();
}

async function emitirFactura(id) {
    const confirmResult = await ajaxHelper.showConfirm(
        '¿Está seguro?',
        'Esta acción emitirá la factura y no podrá ser modificada',
        'Sí, emitir',
        'Cancelar'
    );
    
    if (!confirmResult.isConfirmed) {
        return;
    }
    
    const result = await ajaxHelper.request(`/Facturas/Emitir?id=${id}`, 'POST', {});
    
    if (!result.success || !result.data.success) {
        ajaxHelper.showError(result.error || result.data?.error || 'Error al emitir la factura', result.data?.errors || []);
        return;
    }
    
    ajaxHelper.showSuccess('Factura emitida exitosamente');
    tabla.ajax.reload();
}

function mostrarModalAnular(id) {
    $('#anularFacturaId').val(id);
    $('#motivoAnulacion').val('');
    $('#modalAnularFactura').modal('show');
}

async function confirmarAnular() {
    const id = $('#anularFacturaId').val();
    const motivo = $('#motivoAnulacion').val();
    
    if (!motivo) {
        ajaxHelper.showError('Por favor ingrese el motivo de anulación');
        return;
    }
    
    const data = {
        idFactura: parseInt(id),
        motivo: motivo
    };
    
    const result = await ajaxHelper.request(`/Facturas/Anular?id=${id}`, 'POST', data);
    
    if (!result.success || !result.data.success) {
        ajaxHelper.showError(result.error || result.data?.error || 'Error al anular la factura', result.data?.errors || []);
        return;
    }
    
    ajaxHelper.showSuccess('Factura anulada exitosamente');
    $('#modalAnularFactura').modal('hide');
    tabla.ajax.reload();
}
