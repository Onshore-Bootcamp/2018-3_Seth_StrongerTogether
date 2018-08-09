var x = 150;
var y = 150;
var dx = 2;
var dy = 4;
var WIDTH;
var HEIGHT;
var ctx;
var paddlex;
var paddleh;
var paddlew;
var intervalidId = 0;
var canvasMinX;
var canvasMaxX;
var bricks;
var NROWS;
var NCOLS;
var BRICKWIDTH;
var BRICKHEIGHT;
var PADDING;
var ballr = 10;
var rowcolors = ["#FFFFFF", "#FF0000", "#00FF00", "#0008DB", "#EB0093"];
var paddlecolor = "#FFFFFF";
var ballcolor = "#FFFFFF";
var backcolor = "#000000";

function StartBrickBreaker() {
    ctx = $('#canvas')[0].getContext('2d');
    init();
    init_paddle();
    init_mouse();
    initbricks();
}

function init() {
    WIDTH = $("#canvas").width();
    HEIGHT = $("#canvas").height();
    clearInterval(intervalidId);
    intervalId = setInterval(draw, 10);
}

function circle(x, y, r) {
    ctx.beginPath();
    ctx.arc(x, y, r, 0, Math.PI * 2, true);
    ctx.closePath();
    ctx.fill();
}

function rect(x, y, w, h) {

    ctx.fillRect(x, y, w, h);
}

function clear() {
    ctx.clearRect(0, 0, WIDTH, HEIGHT);
    rect(0, 0, WIDTH, HEIGHT);
}
function init_paddle() {
    paddlex = WIDTH / 2;
    paddleh = 10;
    paddlew = 75;
}
rightDown = false;
leftDown = false;

function onKeyDown(evt) {
    if (evt.keyCode == 39) rightDown = true;
    else if (evt.keyCode == 37) leftDown = true;
}

function onKeyUp(evt) {
    if (evt.keyCode == 39) rightDown = false;
    else if (evt.keyCode == 37) leftDown = false;
}

$(document).keydown(onKeyDown);
$(document).keyup(onKeyUp);

function init_mouse() {
    canvasMinX = $("#canvas").offset().left;
    canvasMaxX = canvasMinX + WIDTH;
}

function onMouseMove(evt) {
    if (evt.pageX > canvasMinX && evt.pageX < canvasMaxX) {
        paddlex = evt.pageX - canvasMinX;
    }
}

$(document).mousemove(onMouseMove);

function initbricks() {
    NROWS = 5;
    NCOLS = 5;
    BRICKWIDTH = (WIDTH / NCOLS) - 1;
    BRICKHEIGHT = 15;
    PADDING = 1;

    bricks = new Array(NROWS);
    for (i = 0; i < NROWS; i++) {
        bricks[i] = new Array(NCOLS);
        for (j = 0; j < NCOLS; j++) {
            bricks[i][j] = 1;
        }
    }
}

function drawBricks() {
    for (var i = 0; i < NROWS; i++) {
        ctx.fillStyle = rowcolors[i];
        
        for (var j = 0; j < NCOLS; j++) {
            if (bricks[i][j] == 1) {
                rect((j * (BRICKWIDTH + PADDING)) + PADDING,
                    (i * (BRICKHEIGHT + PADDING)) + PADDING,
                    BRICKWIDTH, BRICKHEIGHT);
            }
        }
    }
}

function draw() {
    ctx.fillStyle = backcolor;
    clear();
    ctx.fillStyle = ballcolor;
    circle(x, y, ballr);

    if (rightDown) paddlex += 5;
    else if (leftDown) paddlex -= 5
    ctx.fillStyle = paddlecolor;
    rect(paddlex, HEIGHT - paddleh, paddlew, paddleh);

    drawBricks();



    rowheight = BRICKHEIGHT + PADDING;
    colwidth = BRICKWIDTH + PADDING;
    row = Math.floor(y / rowheight);
    col = Math.floor(x / colwidth);

    if (y < NROWS * rowheight && row >= 0 && col >= 0 && bricks[row][col] == 1) {
        dy = -dy;
        bricks[row][col] = 0;
    }

    if (x + dx + ballr > WIDTH || x + dx - ballr < 0)
        dx = -dx;
    if (y + dy - ballr < 0)
        dy = -dy;
    else if (y + dy + ballr > HEIGHT - paddleh) {
        if (x > paddlex && x < paddlex + paddlew) {
            dx = 8 * ((x - (paddlex + paddlew / 2)) / paddlew);
            dy = -dy;
        }

        else if (y + dy + ballr > HEIGHT)
            clearInterval(intervalidId);
    }
    x += dx;
    y += dy;
}