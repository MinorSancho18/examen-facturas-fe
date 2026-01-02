let tabla;
let modoEdicion = false;

$(document).ready(function() {
    inicializarTabla();
    inicializarEventos();
});

function inicializarTabla() {
    tabla = $('#tablaClientes').DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: '/Clientes/Listar',
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
            { data: 'idCliente' },
            { data: 'nombre' },
            { data: 'correo' },
            { 
                data: 'activo',
                render: function(data) {
                    return data ? '<span class="badge bg-success">Activo</span>' : '<span class="badge bg-secondary">Inactivo</span>';
                }
            },
            {
                data: null,
                orderable: false,
                render: function(data, type, row) {
                    return `
                        <button class="btn btn-sm btn-info btnVer" data-id="${row.idCliente}">Ver</button>
                        <button class="btn btn-sm btn-warning btnEditar" data-id="${row.idCliente}">Editar</button>
                        <button class="btn btn-sm btn-danger btnEliminar" data-id="${row.idCliente}">Eliminar</button>
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
    $('#btnNuevoCliente').click(function() {
        modoEdicion = false;
        limpiarFormulario();
        $('#modalClienteTitulo').text('Nuevo Cliente');
        $('#divClienteActivo').hide();
        $('#modalCliente').modal('show');
    });

    $('#btnGuardarCliente').click(guardarCliente);

    $('#tablaClientes').on('click', '.btnVer', function() {
        const id = $(this).data('id');
        verCliente(id);
    });

    $('#tablaClientes').on('click', '.btnEditar', function() {
        const id = $(this).data('id');
        editarCliente(id);
    });

    $('#tablaClientes').on('click', '.btnEliminar', function() {
        const id = $(this).data('id');
        eliminarCliente(id);
    });
}

function limpiarFormulario() {
    $('#clienteId').val('');
    $('#clienteNombre').val('');
    $('#clienteCorreo').val('');
    $('#clienteActivo').prop('checked', true);
}

async function verCliente(id) {
    const result = await ajaxHelper.request(`/Clientes/Obtener?id=${id}`, 'GET');
    
    if (!result.success || !result.data.success) {
        ajaxHelper.showError(result.error || result.data?.error || 'Error al obtener el cliente');
        return;
    }

    const cliente = result.data.data;
    $('#verClienteId').text(cliente.idCliente);
    $('#verClienteNombre').text(cliente.nombre);
    $('#verClienteCorreo').text(cliente.correo);
    $('#verClienteEstado').html(cliente.activo ? '<span class="badge bg-success">Activo</span>' : '<span class="badge bg-secondary">Inactivo</span>');
    $('#modalVerCliente').modal('show');
}

async function editarCliente(id) {
    const result = await ajaxHelper.request(`/Clientes/Obtener?id=${id}`, 'GET');
    
    if (!result.success || !result.data.success) {
        ajaxHelper.showError(result.error || result.data?.error || 'Error al obtener el cliente');
        return;
    }

    const cliente = result.data.data;
    modoEdicion = true;
    $('#clienteId').val(cliente.idCliente);
    $('#clienteNombre').val(cliente.nombre);
    $('#clienteCorreo').val(cliente.correo);
    $('#clienteActivo').prop('checked', cliente.activo);
    $('#divClienteActivo').show();
    $('#modalClienteTitulo').text('Editar Cliente');
    $('#modalCliente').modal('show');
}

async function guardarCliente() {
    const nombre = $('#clienteNombre').val();
    const correo = $('#clienteCorreo').val();
    
    if (!nombre || !correo) {
        ajaxHelper.showError('Por favor complete todos los campos');
        return;
    }

    if (modoEdicion) {
        const id = $('#clienteId').val();
        const activo = $('#clienteActivo').is(':checked');
        const data = {
            idCliente: parseInt(id),
            nombre: nombre,
            correo: correo,
            activo: activo
        };
        
        const result = await ajaxHelper.request(`/Clientes/Actualizar?id=${id}`, 'PUT', data);
        
        if (!result.success || !result.data.success) {
            ajaxHelper.showError(result.error || result.data?.error || 'Error al actualizar el cliente', result.data?.errors || []);
            return;
        }
        
        ajaxHelper.showSuccess('Cliente actualizado exitosamente');
    } else {
        const data = {
            nombre: nombre,
            correo: correo
        };
        
        const result = await ajaxHelper.request('/Clientes/Crear', 'POST', data);
        
        if (!result.success || !result.data.success) {
            ajaxHelper.showError(result.error || result.data?.error || 'Error al crear el cliente', result.data?.errors || []);
            return;
        }
        
        ajaxHelper.showSuccess('Cliente creado exitosamente');
    }
    
    $('#modalCliente').modal('hide');
    tabla.ajax.reload();
}

async function eliminarCliente(id) {
    const confirmResult = await ajaxHelper.showConfirm(
        '¿Está seguro?',
        'Esta acción eliminará el cliente permanentemente',
        'Sí, eliminar',
        'Cancelar'
    );
    
    if (!confirmResult.isConfirmed) {
        return;
    }
    
    const result = await ajaxHelper.request(`/Clientes/Eliminar?id=${id}`, 'DELETE');
    
    if (!result.success || !result.data.success) {
        ajaxHelper.showError(result.error || result.data?.error || 'Error al eliminar el cliente', result.data?.errors || []);
        return;
    }
    
    ajaxHelper.showSuccess('Cliente eliminado exitosamente');
    tabla.ajax.reload();
}
