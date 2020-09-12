let blackIsYou = true;
let whiteIsYou = true;
let isBlackTurn = true;

const getCellElement = function (row, col) {
    return document.getElementById('cell-' + row + '-' + col);
};

const addStone = function (row, col, className) {
    const cell = getCellElement(row, col);
    const stone = document.createElement('div');
    stone.classList.add('stone');
    stone.classList.add(className);
    cell.appendChild(stone);
};

const hasClass = function (element, className) {
    for (let i = 0; i < element.classList.length; i++) {
        if (element.classList[i] === className) {
            return true;
        }
    }
    return false;
};

const removeClassAll = function (className) {
    const elements = document.getElementsByClassName(className);
    for (let i = elements.length; i > 0; i--) {
        elements[i - 1].classList.remove(className);
    }
};

const isMyStone = function (cell) {
    if ((isBlackTurn && hasClass(cell.children[0], 'black')) ||
        (!isBlackTurn && hasClass(cell.children[0], 'white'))) {
        return true;
    }
    return false;
};

const isValidLine = function (row, col, addRow, addCol) {
    let rowCount = addRow;
    let colCount = addCol;
    for (let i = 0; i < 7; i++) {
        const cell = getCellElement((row - rowCount), (col - colCount));
        if (!cell || !cell.children.length) {
            return false;
        }
        if (isMyStone(cell)) {
            if (rowCount === addRow && colCount === addCol) {
                return false;
            } else {
                return true;
            }
        }
        rowCount = rowCount + addRow;
        colCount = colCount + addCol;
    }
    return false;
};

const isValidCell = function (row, col) {
    if (isValidLine(row, col, -1, -1) ||
        isValidLine(row, col, -1, 0) ||
        isValidLine(row, col, -1, 1) ||
        isValidLine(row, col, 0, -1) ||
        isValidLine(row, col, 0, 1) ||
        isValidLine(row, col, 1, -1) ||
        isValidLine(row, col, 1, 0) ||
        isValidLine(row, col, 1, 1)) {
        return true;
    }
};

const turnStones = function (cells) {
    for (let i = 0; i < cells.length; i++) {
        const cell = document.getElementById(cells[i]);
        if (isBlackTurn) {
            cell.children[0].classList.remove('white');
            cell.children[0].classList.add('black');
            cell.children[0].classList.add('turn-effect');
            setTimeout(function () {
                cell.children[0].classList.remove('turn-effect');
            }, 200);
        } else {
            cell.children[0].classList.remove('black');
            cell.children[0].classList.add('white');
            cell.children[0].classList.add('turn-effect');
            setTimeout(function () {
                cell.children[0].classList.remove('turn-effect');
            }, 200);
        }
    }
};

const turnLineStones = function (row, col, addRow, addCol) {
    let rowCount = addRow;
    let colCount = addCol;
    let cells = [];
    for (let i = 0; i < 7; i++) {
        const cell = getCellElement((row - rowCount), (col - colCount));
        if (!cell || !cell.children.length) {
            return;
        }
        if (isMyStone(cell)) {
            if (rowCount !== addRow || colCount !== addCol) {
                turnStones(cells);
            }
            return;
        }
        cells.push(cell.id);
        rowCount = rowCount + addRow;
        colCount = colCount + addCol;
    }
    return;
};

const turnAllStones = function (row, col) {
    turnLineStones(row, col, -1, -1);
    turnLineStones(row, col, -1, 0);
    turnLineStones(row, col, -1, 1);
    turnLineStones(row, col, 0, -1);
    turnLineStones(row, col, 0, 1);
    turnLineStones(row, col, 1, -1);
    turnLineStones(row, col, 1, 0);
    turnLineStones(row, col, 1, 1);
};

const searchValidCells = function () {
    for (let row = 0; row < 8; row++) {
        for (let col = 0; col < 8; col++) {
            const cell = getCellElement(row, col);
            if (cell.children.length) continue;
            if (isValidCell(row, col)) {
                cell.classList.add('valid');
            }
        }
    }
};

const updateScore = function () {
    const black = document.getElementsByClassName('black');
    const white = document.getElementsByClassName('white');
    const all = black.length + white.length;
    const blackRatio = black.length * 100 / all;
    const whiteRatio = white.length * 100 / all;
    document.getElementById('black-score').innerText = black.length;
    document.getElementById('white-score').innerText = white.length;
    document.getElementById('black-bar').style.width = blackRatio + '%';
    document.getElementById('white-bar').style.width = whiteRatio + '%';
};

const showMessageNoPlace = function () {
    if (isBlackTurn) {
        document.getElementById('message').innerText = "Black can't put.";
    } else {
        document.getElementById('message').innerText = "White can't put.";
    }
    document.getElementById('message-container').style.display = 'block';
};

const isTheEnd = function () {
    const stones = document.getElementsByClassName('stone');
    if (stones.length === 8 * 8) {
        return true;
    }
    const black = document.getElementsByClassName('black');
    if (black.length === 0) {
        return true;
    }
    const white = document.getElementsByClassName('white');
    if (white.length === 0) {
        return true;
    }
};

const endGame = function () {
    document.getElementById('white-label').classList.remove('turn');
    document.getElementById('black-label').classList.remove('turn');
    const black = document.getElementsByClassName('black');
    const white = document.getElementsByClassName('white');
    if (black.length > white.length) {
        document.getElementById('message').innerText = 'Black win.';
    } else if (black.length < white.length) {
        document.getElementById('message').innerText = 'White win.';
    } else {
        document.getElementById('message').innerText = 'Draw.';
    }
    document.getElementById('message-container').style.display = 'block';
};

const clickCorner = function (elements) {
    for (let i = 0; i < elements.length; i++) {
        const splitedId = elements[i].id.split('-');
        if (splitedId[1] === '0' && splitedId[2] === '0' ||
            splitedId[1] === '0' && splitedId[2] === '7' ||
            splitedId[1] === '7' && splitedId[2] === '0' ||
            splitedId[1] === '7' && splitedId[2] === '7') {
            elements[i].click();
            return true;
        }
    }
    return false;
};

const clickCpu = function (elements) {
    if ((isBlackTurn && !blackIsYou) || (!isBlackTurn && !whiteIsYou)) {
        if (!clickCorner(elements)) {
            elements[Math.floor(Math.random() * elements.length)].click();
        }
    }
};

const updateTurnLabels = function () {
    if (isBlackTurn) {
        document.getElementById('white-label').classList.remove('turn');
        document.getElementById('black-label').classList.add('turn');
    } else {
        document.getElementById('black-label').classList.remove('turn');
        document.getElementById('white-label').classList.add('turn');
    }
};

const updateTurnSecond = function (updateBlack) {
    isBlackTurn = updateBlack;
    updateTurnLabels();
    searchValidCells();
    const elements = document.getElementsByClassName('valid');
    if (elements.length === 0) {
        showMessageNoPlace();
        setTimeout(function () {
            endGame();
        }, 2000);
    } else {
        clickCpu(elements);
    }
};

const updateTurn = function (updateBlack) {
    isBlackTurn = updateBlack;
    updateTurnLabels();
    searchValidCells();
    const elements = document.getElementsByClassName('valid');
    if (elements.length === 0) {
        showMessageNoPlace();
        setTimeout(function () {
            document.getElementById('message-container').style.display = '';
            updateTurnSecond(!isBlackTurn);
        }, 2000);
    } else {
        clickCpu(elements);
    }
};

const addInitStones = function () {
    addStone(3, 3, 'white');
    addStone(3, 4, 'black');
    addStone(4, 3, 'black');
    addStone(4, 4, 'white');
};

const init = function () {
    blackIsYou = true;
    whiteIsYou = true;
    addInitStones();
    updateScore();
    updateTurn(true);
    document.getElementById('start-container').style.display = '';
};

const onClickCell = function () {
    if (!hasClass(this, 'valid')) return;
    removeClassAll('valid');
    const splitedId = this.id.split('-');
    if (isBlackTurn) {
        addStone(splitedId[1], splitedId[2], 'black');
    } else {
        addStone(splitedId[1], splitedId[2], 'white');
    }
    turnAllStones(splitedId[1], splitedId[2]);
    updateScore();
    setTimeout(function () {
        if (isTheEnd()) {
            endGame();
            return;
        }
        updateTurn(!isBlackTurn);
    }, 400);
};

const onClickReset = function () {
    document.getElementById('message-container').style.display = '';
    removeClassAll('valid');
    const stones = document.getElementsByClassName('stone');
    for (let i = stones.length; i > 0; i--) {
        const parent = stones[i - 1].parentNode;
        parent.removeChild(stones[i - 1]);
    }
    init();
};

const onClickStart = function () {
    const black = document.getElementsByName('black');
    if (black[0].checked) {
        blackIsYou = true;
    } else {
        blackIsYou = false;
    }
    const white = document.getElementsByName('white');
    if (white[0].checked) {
        whiteIsYou = true;
    } else {
        whiteIsYou = false;
    }
    document.getElementById('start-container').style.display = 'none';
    updateTurn(true);
};

const addEvents = function () {
    const cells = document.getElementsByClassName('cell');
    for (let i = 0; i < cells.length; i++) {
        cells[i].addEventListener('click', onClickCell, false);
    }
    const reset = document.getElementById('reset');
    reset.addEventListener('click', onClickReset, false);
    const start = document.getElementById('start');
    start.addEventListener('click', onClickStart, false);
};

window.onload = function () {
    addEvents();
    init();
};