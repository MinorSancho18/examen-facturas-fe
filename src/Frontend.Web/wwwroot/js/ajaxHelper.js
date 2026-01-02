const ajaxHelper = {
    request: async function(url, method = 'GET', data = null) {
        try {
            const options = {
                method: method,
                headers: { 'Content-Type': 'application/json' }
            };
            
            if (data && method !== 'GET') {
                options.body = JSON.stringify(data);
            }
            
            const response = await fetch(url, options);
            const result = await response.json();
            
            if (!response.ok) {
                return {
                    success: false,
                    error: result.error || 'Error desconocido',
                    errors: result.errors || []
                };
            }
            
            return {
                success: true,
                data: result
            };
        } catch (error) {
            return {
                success: false,
                error: error.message
            };
        }
    },
    
    showSuccess: function(message) {
        Swal.fire('Éxito', message, 'success');
    },
    
    showError: function(error, errors = []) {
        let msg = error;
        if (errors.length > 0) {
            msg += '<ul>' + errors.map(e => '<li>' + e + '</li>').join('') + '</ul>';
        }
        Swal.fire('Error', msg, 'error');
    },
    
    showConfirm: function(title, text, confirmButtonText = 'Sí', cancelButtonText = 'No') {
        return Swal.fire({
            title: title,
            text: text,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: confirmButtonText,
            cancelButtonText: cancelButtonText
        });
    }
};
