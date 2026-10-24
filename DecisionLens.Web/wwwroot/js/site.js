// Shared behaviour for every page.
// Any form with a data-confirm attribute asks for confirmation before it is submitted.
document.addEventListener('submit', function (event) {
    var form = event.target;
    var message = form.getAttribute ? form.getAttribute('data-confirm') : null;

    if (message && !window.confirm(message)) {
        event.preventDefault();
    }
});