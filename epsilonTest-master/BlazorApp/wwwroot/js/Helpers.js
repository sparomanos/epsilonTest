
//Sweet Alert Message
//https://sweetalert2.github.io/

function DisplayMessage(title,msg,type) {
    Swal.fire(
        title,
        msg,
        type
    )
}


function DisplayConfirmMessage(confirm, title, message, type) {
    return new Promise((resolve) => {
        Swal.fire({
            title: title,
            text: message,
            icon: type,
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: confirm
        }).then((result) => {
            if (result.value) {
                resolve(true)
            }
            else {
                resolve(false)
            }
        });
    });
}