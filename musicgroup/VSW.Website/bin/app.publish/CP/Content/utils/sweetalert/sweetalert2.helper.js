function showSwalWarning(message, title, callback) {
    Swal.fire({
        icon: 'warning',
        title: title,
        html: message,
        confirmButtonText: 'OK',
        confirmButtonColor: '#3085d6'
    }).then((result) => {
        if (callback !== undefined && callback !== null) {
            callback();
        }
    });
}

function showSwalSuccess(message, title, callback) {
    Swal.fire({
        title: title,
        html: message,
        icon: 'success',
        confirmButtonText: 'OK',
        confirmButtonColor: '#3085d6'
    }).then((result) => {
        if (callback !== undefined && callback !== null) {
            callback();
        }
    });
}

function showSwalQuestion(message, title, callback) {
    Swal.fire({
        title: title,
        html: message,
        icon: 'info',
        showCancelButton: true,
        cancelButtonText: 'Không',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Có',
        confirmButtonColor: '#3085d6'
    }).then((result) => {
        if (callback !== undefined && callback !== null) {
            callback(result.value);
        }
    });
}
//alert
function Alert(title, content) {
    Swal.fire({
        icon: 'warning',
        title: title,
        html: content,
        confirmButtonText: 'OK',
        confirmButtonColor: '#3085d6'
    });
}
function Redirect(title, content, redirect) {
    Swal.fire({
        title: title,
        html: content,
        icon: 'info',
        confirmButtonText: 'OK',
        confirmButtonColor: '#3085d6'
    }).then((result) => {
        if (result.value) {
            location.href = redirect;
        }
    })
}
function Confirm(title, content, redirect) {
    Swal.fire({
        title: title,
        html: content,
        icon: 'info',
        showCancelButton: true,
        cancelButtonText: 'Không',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Có',
        confirmButtonColor: '#3085d6'
    }).then((result) => {
        if (result.value) {
            location.href = redirect;
        }
    })
}
function Success(title, content, redirect) {
    Swal.fire({
        title: title,
        html: content,
        icon: 'success',
        confirmButtonText: 'OK',
        confirmButtonColor: '#3085d6'
    }).then((result) => {
        if (result.value) {
            if (redirect != '') {
                location.href = redirect
            }
        }
    })
}