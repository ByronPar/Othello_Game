let NEGRO = true;
let BLANCO = true;
let turnoNegro = true;

const obtenerElementoCelda = function (fila, columna) {
    return document.getElementById('cell-' + fila + '-' + columna);
};

const agregarFicha = function (fila, columna, nombreClase) {
    const celda = obtenerElementoCelda(fila, columna);
    const ficha = document.createElement('div');
    ficha.classList.add('stone');
    ficha.classList.add(nombreClase);
    celda.appendChild(ficha);
};

const tieneClase = function (elemento, nombreClase) {
    for (let i = 0; i < elemento.classList.length; i++) {
        if (elemento.classList[i] === nombreClase) {
            return true;
        }
    }
    return false;
};

const removerTodaClase = function (nombreClase) {
    const elementos = document.getElementsByClassName(nombreClase);
    for (let i = elementos.length; i > 0; i--) {
        elementos[i - 1].classList.remove(nombreClase);
    }
};

const esMiFicha = function (celda) {
    if ((turnoNegro && tieneClase(celda.children[0], 'black')) ||
        (!turnoNegro && tieneClase(celda.children[0], 'white'))) {
        return true;
    }
    return false;
};

const lineaValida = function (fila, columna, agregarFila, agregarColumna) {
    let conteoFila = agregarFila;
    let conteoColumna = agregarColumna;
    for (let i = 0; i < 7; i++) {
        const celda = obtenerElementoCelda((fila - conteoFila), (columna - conteoColumna));
        if (!celda || !celda.children.length) {
            return false;
        }
        if (esMiFicha(celda)) {
            if (conteoFila === agregarFila && conteoColumna === agregarColumna) {
                return false;
            } else {
                return true;
            }
        }
        conteoFila = conteoFila + agregarFila;
        conteoColumna = conteoColumna + agregarColumna;
    }
    return false;
};

const celdaValida = function (fila, columna) {
    if (lineaValida(fila, columna, -1, -1) ||
        lineaValida(fila, columna, -1, 0) ||
        lineaValida(fila, columna, -1, 1) ||
        lineaValida(fila, columna, 0, -1) ||
        lineaValida(fila, columna, 0, 1) ||
        lineaValida(fila, columna, 1, -1) ||
        lineaValida(fila, columna, 1, 0) ||
        lineaValida(fila, columna, 1, 1)) {
        return true;
    }
};

const girarFicha = function (celdas) {
    for (let i = 0; i < celdas.length; i++) {
        const celda = document.getElementById(celdas[i]);
        if (turnoNegro) {
            celda.children[0].classList.remove('white');
            celda.children[0].classList.add('black');
            celda.children[0].classList.add('turn-effect');
            setTimeout(function () {
                celda.children[0].classList.remove('turn-effect');
            }, 200);
        } else {
            celda.children[0].classList.remove('black');
            celda.children[0].classList.add('white');
            celda.children[0].classList.add('turn-effect');
            setTimeout(function () {
                celda.children[0].classList.remove('turn-effect');
            }, 200);
        }
    }
};

const girarLineaFicha = function (fila, columna, agregarFila, agregarColumna) {
    let conteoFila = agregarFila;
    let conteoColumna = agregarColumna;
    let celdas = [];
    for (let i = 0; i < 7; i++) {
        const celda = obtenerElementoCelda((fila - conteoFila), (columna - conteoColumna));
        if (!celda || !celda.children.length) {
            return;
        }
        if (esMiFicha(celda)) {
            if (conteoFila !== agregarFila || conteoColumna !== agregarColumna) {
                girarFicha(celdas);
            }
            return;
        }
        celdas.push(celda.id);
        conteoFila = conteoFila + agregarFila;
        conteoColumna = conteoColumna + agregarColumna;
    }
    return;
};

const girarTodaFicha = function (row, col) {
    girarLineaFicha(row, col, -1, -1);
    girarLineaFicha(row, col, -1, 0);
    girarLineaFicha(row, col, -1, 1);
    girarLineaFicha(row, col, 0, -1);
    girarLineaFicha(row, col, 0, 1);
    girarLineaFicha(row, col, 1, -1);
    girarLineaFicha(row, col, 1, 0);
    girarLineaFicha(row, col, 1, 1);
};

const buscarCeldasValidas = function () {
    for (let fila = 0; fila < 8; fila++) {
        for (let columna = 0; columna < 8; columna++) {
            const celda = obtenerElementoCelda(fila, columna);
            if (celda.children.length) continue;
            if (celdaValida(fila, columna)) {
                celda.classList.add('valid');
            }
        }
    }
};

const actualizarPuntuacion = function () {
    const negro = document.getElementsByClassName('black');
    const blanco = document.getElementsByClassName('white');
    const todo = negro.length + blanco.length;   // cantidad de fichas totales que hay actualmente
    const radioNegro = negro.length * 100 / todo;
    const radioBlanco = blanco.length * 100 / todo;
    document.getElementById('black-score').innerText = negro.length; //para colocar la barra de cuanto lleva cada jugador
    document.getElementById('white-score').innerText = blanco.length;
    document.getElementById('black-bar').style.width = radioNegro + '%'; // la barra pintada de porcentaje de cuanto lleva cada jugador
    document.getElementById('white-bar').style.width = radioBlanco + '%';
};

const showMsjNoHayLugar = function () {
    if (turnoNegro) {
        document.getElementById('message').innerText = "Negro no puede jugar";
    } else {
        document.getElementById('message').innerText = "Blanco no puede jugar";
    }
    document.getElementById('message-container').style.display = 'block';
};

const esElFinal = function () {
    const ficha = document.getElementsByClassName('stone');
    if (ficha.length === 8 * 8) {
        return true;
    }
    const negro = document.getElementsByClassName('black');
    if (negro.length === 0) {
        return true;
    }
    const blanco = document.getElementsByClassName('white');
    if (blanco.length === 0) {
        return true;
    }
};

const finJuego = function () {
    document.getElementById('white-label').classList.remove('turn');
    document.getElementById('black-label').classList.remove('turn');
    const negro = document.getElementsByClassName('black');
    const blanco = document.getElementsByClassName('white');
    if (negro.length > blanco.length) {
        document.getElementById('message').innerText = 'Ficha negro Gano';
    } else if (negro.length < blanco.length) {
        document.getElementById('message').innerText = 'Ficha Blanca Gano';
    } else {
        document.getElementById('message').innerText = 'Draw.';
    }
    document.getElementById('message-container').style.display = 'block';
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
    if ((turnoNegro && !NEGRO) || (!turnoNegro && !BLANCO)) {
        if (!clickEsquina(elementos)) {
            elementos[Math.floor(Math.random() * elementos.length)].click();
        }
    }
};

const actualizarTurnoLabel = function () {
    if (turnoNegro) {
        document.getElementById('white-label').classList.remove('turn');
        document.getElementById('black-label').classList.add('turn');
    } else {
        document.getElementById('black-label').classList.remove('turn');
        document.getElementById('white-label').classList.add('turn');
    }
};

const actualizarTurnoSegundo = function (actualizarNegro) {
    turnoNegro = actualizarNegro;
    actualizarTurnoLabel();
    buscarCeldasValidas();
    const elementos = document.getElementsByClassName('valid');
    if (elementos.length === 0) {
        showMsjNoHayLugar();
        setTimeout(function () {
            finJuego();
        }, 2000);
    } else {
        clickCpu(elementos);
    }
};

const actualizarTurno = function (actualizarNegro) {
    turnoNegro = actualizarNegro;    //inicia como true el negro siempre
    actualizarTurnoLabel();
    buscarCeldasValidas();
    const elementos = document.getElementsByClassName('valid');
    if (elementos.length === 0) {
        showMsjNoHayLugar();
        setTimeout(function () {
            document.getElementById('message-container').style.display = '';
            actualizarTurnoSegundo(!turnoNegro);
        }, 2000);
    } else {
        clickCpu(elementos);
    }
};

const agregarFichasIniciales = function () {
    agregarFicha(3, 3, 'white');
    agregarFicha(3, 4, 'black');
    agregarFicha(4, 3, 'black');
    agregarFicha(4, 4, 'white');
};

const agregarFichasInicialesXML = function (elementos) {
    for (var i = 0; i < elementos.length; i++) {
        switch (i[1]) {
            case 'A':
                agregarFicha(i[0], 0, i[2]);
            case 'B':
                agregarFicha(i[0], 1, i[2]);
            case 'C':
                agregarFicha(i[0], 2, i[2]);
            case 'D':
                agregarFicha(i[0], 3, i[2]);
            case 'E':
                agregarFicha(i[0], 4, i[2]);
            case 'F':
                agregarFicha(i[0], 5, i[2]);
            case 'G':
                agregarFicha(i[0], 6, i[2]);
            case 'H':
                agregarFicha(i[0], 7, i[2]);
        }
        
    }
}

const init = function () {
    NEGRO = true;
    BLANCO = true;
    agregarFichasIniciales();
    actualizarPuntuacion();
    actualizarTurno(true);
    document.getElementById('start-container').style.display = '';
};

const initCargaXML = function (elementos,turno) {
    NEGRO = true;
    BLANCO = true;
    agregarFichasInicialesXML(elementos);
    actualizarPuntuacion();
    if (turno == 'negro') {
        actualizarTurno(true);
    } else {
        actualizarTurno(false);
    }
    document.getElementById('start-container').style.display = '';
}

const clickCelda = function () {
    if (!tieneClase(this, 'valid')) return;
    removerTodaClase('valid');
    const splitedId = this.id.split('-');
    if (turnoNegro) {
        agregarFicha(splitedId[1], splitedId[2], 'black');
    } else {
        agregarFicha(splitedId[1], splitedId[2], 'white');
    }
    girarTodaFicha(splitedId[1], splitedId[2]);
    actualizarPuntuacion();
    setTimeout(function () {
        if (esElFinal()) {
            finJuego();
            return;
        }
        actualizarTurno(!turnoNegro);
    }, 400);
};

const clickReiniciar = function () {
    document.getElementById('message-container').style.display = '';
    removerTodaClase('valid');
    const fichas = document.getElementsByClassName('stone');
    for (let i = fichas.length; i > 0; i--) {
        const padre = fichas[i - 1].parentNode;
        padre.removeChild(fichas[i - 1]);
    }
    init();
};

const clickIniciar = function () {
    const negro = document.getElementsByName('black');
    if (negro[0].checked) {
        NEGRO = true;
        BLANCO = false;
    } else {
        NEGRO = false;
        BLANCO = true;
    }
    document.getElementById('start-container').style.display = 'none';
    actualizarTurno(true);
};

const clickCargar = function () {
    var datos = document.getElementById('url').value;
    var xhr = new XMLHttpRequest();
    xhr.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            cargarXML(this);
        }
    };
    xhr.open("GET", datos, true);
    xhr.send();
}

const cargarXML = function (xml) {
    var docXML = xml.responseXML;
    var elementos = []
    var fichas = docXML.getElementByTagName("ficha");
    for (var i = 0; i < fichas.length; i++) {
        elemento = []
        elemento[0] = ficha[i].getElementByTagName("fila")[0].textContent;
        elemento[1] = ficha[i].getElementByTagName("columna")[0].textContent;
        elemento[2] = ficha[i].getElementByTagName("color")[0].textContent;
        elementos.push(elemento);
    }
    var turno = docXML.getElementByTagName("siguienteTiro");
    for (var i = 0; i < turno.length; i++) {
        turno2 = turno[i].getElementByTagName("color")[0].textContent;
    }
    initCargaXML(elementos,turno2);
}

const agregarEventos = function () {
    const celdas = document.getElementsByClassName('cell');
    for (let i = 0; i < celdas.length; i++) {
        celdas[i].addEventListener('click', clickCelda, false);
    }
    const reiniciar = document.getElementById('reiniciar');
    reiniciar.addEventListener('click', clickReiniciar, false);
    const iniciar = document.getElementById('iniciar');
    iniciar.addEventListener('click', clickIniciar, false);
    const cargar = document.getElementById('cargar');
    cargar.addEventListener('click', clickCargar, false);
};

window.onload = function () {
    agregarEventos();
    init();
};