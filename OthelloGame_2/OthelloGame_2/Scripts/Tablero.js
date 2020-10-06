const tieneClase = function (elemento, nombreClase) {
    for (let i = 0; i < elemento.classList.length; i++) {
        if (elemento.classList[i] === nombreClase) {
            return true;
        }
    }
    return false;
};

function CalculosControlador(fila, id_columna) {
    var id_fila = String(fila);
    var id_columna2 = String(id_columna);
    url = url.replace('color2', id_fila);
    url = url.replace('id_columna2', id_columna2);
    window.location.href = url;
}

const clickCelda = function () {
    if (!tieneClase(this, 'valido')) return;
    const valores = this.id.split('-');
    CalculosControlador(valores[1],valores[2]);
};

const agregarEventos = function () {
    const celdas = document.getElementsByTagName('td');
    for (let i = 0; i < celdas.length; i++) {
        if (tieneClase(celdas[i],'celda')) {
            celdas[i].addEventListener('click', clickCelda, false);
        }
    }
};

window.onload = function () {
    agregarEventos();
};