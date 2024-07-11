window.showModal = (id) => {
    var modal = new bootstrap.Modal(document.getElementById(id));
    modal.show();
};

window.hideModal = (id) => {
    var modal = bootstrap.Modal.getInstance(document.getElementById(id));
    if (modal) {
        modal.hide();
    }
};
