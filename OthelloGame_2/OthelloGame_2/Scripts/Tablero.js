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
    url = url.replace('tocaTurno', turno)
    window.location.href = url;
}

const clickCelda = function () {
    if (!tieneClase(this, 'valido')) return;
    const valores = this.id.split('-');
    CalculosControlador(valores[1], valores[2]);
};

const agregarEventos = function () {
    const celdas = document.getElementsByTagName('td');
    for (let i = 0; i < celdas.length; i++) {
        if (tieneClase(celdas[i], 'celda')) {
            celdas[i].addEventListener('click', clickCelda, false);
        }
    }
};

const clickEsquina = function (elements) {
    for (let i = 0; i < elements.length; i++) {
        const splitID = elements[i].id.split('-');
        if (splitID[1] === '0' && splitID[2] === '0' ||
            splitID[1] === '0' && splitID[2] === '7' ||
            splitID[1] === '7' && splitID[2] === '0' ||
            splitID[1] === '7' && splitID[2] === '7') {
            elements[i].click();
            return true;
        }
    }
    return false;
};

const clickCpu = function (elementos) {
    if (!clickEsquina(elementos)) {
        elementos[Math.floor(Math.random() * elementos.length)].click();
    }
};

window.onload = function () {
    agregarEventos();
    //if (turno == jugador2) {
    //    setTimeout(function () {
    //        const elementos = document.getElementsByClassName('valido');
    //        clickCpu(elementos);;
    //    }, 5000);
    //}
};