#!/bin/bash
# Script to download required JavaScript libraries
# Run this script from the project root directory

cd src/Frontend.Web/wwwroot/lib

echo "Downloading JavaScript libraries..."

# Download jQuery 3.7.1
echo "Downloading jQuery 3.7.1..."
curl -sL https://code.jquery.com/jquery-3.7.1.min.js -o jquery/jquery.min.js

# Download Bootstrap 5.3.3
echo "Downloading Bootstrap 5.3.3..."
curl -sL https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css -o bootstrap/css/bootstrap.min.css
curl -sL https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js -o bootstrap/js/bootstrap.bundle.min.js

# Download DataTables 2.0
echo "Downloading DataTables 2.0..."
curl -sL https://cdn.datatables.net/2.0.0/css/dataTables.bootstrap5.min.css -o datatables/css/dataTables.bootstrap5.min.css
curl -sL https://cdn.datatables.net/2.0.0/js/jquery.dataTables.min.js -o datatables/js/jquery.dataTables.min.js
curl -sL https://cdn.datatables.net/2.0.0/js/dataTables.bootstrap5.min.js -o datatables/js/dataTables.bootstrap5.min.js

# Download SweetAlert2
echo "Downloading SweetAlert2..."
curl -sL https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.all.min.js -o sweetalert2/sweetalert2.all.min.js
curl -sL https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css -o sweetalert2/sweetalert2.min.css

# Download jQuery Validation
echo "Downloading jQuery Validation..."
curl -sL https://cdn.jsdelivr.net/npm/jquery-validation@1.19.5/dist/jquery.validate.min.js -o jquery-validation/jquery.validate.min.js
curl -sL https://cdn.jsdelivr.net/npm/jquery-validation-unobtrusive@4.0.0/dist/jquery.validate.unobtrusive.min.js -o jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js

echo "All libraries downloaded successfully!"
