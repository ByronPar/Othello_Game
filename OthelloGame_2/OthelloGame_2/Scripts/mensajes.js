window.setTimeout(function () {
    $(".alert").fadeTo(1500, 0).slideDown(1000, function () {
        $(this).remove();
    });
}, 2700);  // 3 segundos y desaparece