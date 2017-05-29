var isMobile = false; //initiate as false
// device detection
if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent) || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4))) isMobile = true;

var isPaused = false;
var timeRemaining = null;
var gameTime = null;
var numItems = null;
var weapon = 1;
var enemy = null;
var $all = $(".hole");
var monsters = ["monster1", "monster1", "monster2", "monster2", "monster3", "monster3", "monster4"];
var justAdded = null;
var game = {
    score: GameScore
}
var music = $("#music")[0];

var poof = $("#poof")[0];

watch(game, "score", function () {
    $('#score span').html(game.score);
});
function startTimer(duration, display) {
    var timer = duration,
        minutes, seconds;

    setInterval(function () {
        if (!isPaused) {
            minutes = parseInt(timer / 60, 10);
            seconds = parseInt(timer % 60, 10);
            minutes = minutes < 10 ? "0" + minutes : minutes;
            seconds = seconds < 10 ? "0" + seconds : seconds;

            display.text(minutes + ":" + seconds);
            if (--timer <= 0) {
                isPaused = true;
                localStorage.setItem('timeRemaining', 300);
                endGame();
                // timer = duration;
            }
            timeRemaining = timer;
            localStorage.setItem('timeRemaining', timeRemaining);
        } else {
            timeRemaining = timer;
        }
    }, 1000);
}
var success = $("#mysoundclip")[0];

function endGame() {
                    if (isDemo) {
                        window.location.replace("/Games/ActiveExplainer");
                        return;
                    }
    $.custom.Server.SaveGameScore(game.score, 1, function (res) { // success
        if (res && res.success) {
            setTimeout(function () {
                $('.wrapper').fadeOut(2000, function () {
                    window.location.replace("/Games/ActiveGameAfter?status=outoftime");
                });
            }, 2000);
        }
        else { // fail

        }
    }, function () { // fail

    });

}
jQuery.fn.center = function () {
    this.css("position", "absolute");
    this.css("top", Math.max(0, (($(window).height() - $(this).outerHeight()) / 2) +
        $(window).scrollTop()) + "px");
    this.css("left", Math.max(0, (($(window).width() - $(this).outerWidth()) / 2) +
        $(window).scrollLeft()) + "px");
    return this;
}


$(window).on('resize', function () {
    $('.game').center();
});


loader.addCompletionListener(function () {
    var time = RemainingTime;

    display = $('#time');
    startTimer(time, display);

    if (isMobile == true) {
        $('body').addClass('mobile');
        $('.windows8').hide();
        $('#letsgo').addClass('show');
        $('#letsgo').click(function (event) {
            $('#preloader').fadeOut(1000);
            $('.wrapper').fadeIn(2000);
            music.play();
        });

    } else {

        $('#preloader').fadeOut(1000);
        $('.wrapper').fadeIn(2000);
        $('.game').center();
        music.play();
    }
});


$(document).ready(function () {
    loader.start();
    $('.exit').click(function(event) {
        /* Act on the event */
        isPaused = false;
        $('#explainer').hide();
    });
    $('.pause').click(function(event) {
        /* Act on the event */
        isPaused = true;
        $('#explainer').show();
    });
    $('.gamewrap').on('click', '.item', function (event) {
        item = $(this);
        itemClue = item.data('clue');
        event.preventDefault();
        /* Act on the event */
    });


});


// fog

console.clear();

canvasWidth = 1600;
canvasHeight = 200;

pCount = 0;


pCollection = new Array();

var puffs = 1;
var particlesPerPuff = 2000;
var img = 'https://s3-us-west-2.amazonaws.com/s.cdpn.io/85280/smoke2.png';

var smokeImage = new Image();
smokeImage.src = img;

for (var i1 = 0; i1 < puffs; i1++) {
    var puffDelay = i1 * 1500; //300 ms between puffs

    for (var i2 = 0; i2 < particlesPerPuff; i2++) {
        addNewParticle((i2 * 50) + puffDelay);
    }
}


draw(new Date().getTime(), 3000)



function addNewParticle(delay) {

    var p = {};
    p.top = canvasHeight;
    p.left = randBetween(-200, 800);

    p.start = new Date().getTime() + delay;
    p.life = 8000;
    p.speedUp = 30;


    p.speedRight = randBetween(0, 20);

    p.rot = randBetween(-1, 1);
    p.red = Math.floor(randBetween(0, 255));
    p.blue = Math.floor(randBetween(0, 255));
    p.green = Math.floor(randBetween(0, 255));


    p.startOpacity = .3
    p.newTop = p.top;
    p.newLeft = p.left;
    p.size = 200;
    p.growth = 10;

    pCollection[pCount] = p;
    pCount++;


}

function draw(startT, totalT) {
    //Timing
    var timeDelta = new Date().getTime() - startT;
    var stillAlive = false;

    //Grab and clear the canvas
    var c = document.getElementById("myCanvas");
    var ctx = c.getContext("2d");
    ctx.clearRect(0, 0, c.width, c.height);
    c.width = c.width;

    //Loop through particles
    for (var i = 0; i < pCount; i++) {
        //Grab the particle
        var p = pCollection[i];

        //Timing
        var td = new Date().getTime() - p.start;
        var frac = td / p.life

        if (td > 0) {
            if (td <= p.life) { stillAlive = true; }

            //attributes that change over time
            var newTop = p.top - (p.speedUp * (td / 1000));
            var newLeft = p.left + (p.speedRight * (td / 1000));
            var newOpacity = Math.max(p.startOpacity * (0.7 - frac), 0);

            var newSize = p.size + (p.growth * (td / 1000));
            p.newTop = newTop;
            p.newLeft = newLeft;

            //Draw!
            ctx.fillStyle = 'rgba(150,150,150,' + newOpacity + ')';
            ctx.globalAlpha = newOpacity;
            ctx.drawImage(smokeImage, newLeft, newTop, newSize, newSize);
        }
    }



    //Repeat if there's still a living particle
    if (stillAlive) {
        requestAnimationFrame(function () { draw(startT, totalT); });
    } else {
        clog(timeDelta + ": stopped");
    }
}

function randBetween(n1, n2) {
    var r = (Math.random() * (n2 - n1)) + n1;
    return r;
}

function randOffset(n, variance) {
    //e.g. variance could be 0.1 to go between 0.9 and 1.1
    var max = 1 + variance;
    var min = 1 - variance;
    var r = Math.random() * (max - min) + min;
    return n * r;
}

function clog(s) {
    console.log(s);
}




function shuffle(array) {
    var m = array.length,
        t, i;
    while (m) {
        i = Math.floor(Math.random() * m--);
        t = array[m];
        array[m] = array[i];
        array[i] = t;
    }
    return array;
}

function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

var level1 = 5000;
var level2 = 4000;
var level3 = 3000;
var level4 = 2000;
var level5 = 1000;
var level = level1;
var addEnemy = function () {
    numItems = $('.enemy').length;
    if (numItems <= 10) {

        var randHole = shuffle($all).slice(0, 1);
        while ($('.pos' + randHole.data('hole'))[0]) {
            randHole = shuffle($all).slice(0, 1);
            console.log('duplicate');
        }
        // if (randHole.data('hasmole') == false) {
        enemy = '<div class="enemy pos' + randHole.data('hole') + ' ' + monsters[~~(Math.random() * monsters.length)] + '" data-click="0" data-position="' + randHole.data('hole') + '"></div>';
        $('#stage').append(enemy);
        $('.pos' + randHole.data('hole')).animate({
            top: "-=100px",
            height: 100
        }, 300);

        setTimeout(function () {
            $('.pos' + randHole.data('hole')).animate({
                top: "+=100px",
                height: 0
            }, 300);
            setTimeout(function () {
                $('.pos' + randHole.data('hole')).remove();
            }, 500)

        }, level)

        // } else {
        //     // addEnemy();
        // }
    }
}

var launchEnemies = function () {
    addEnemy()
    var timeoutID = window.setTimeout(launchEnemies, Math.random() * 3000);
}



$(document).ready(function () {
    setInterval(function () {
        if (timeRemaining >= 240) {
            level = level1;
        } else if (timeRemaining >= 180 && timeRemaining <= 239) {
            level = level2;
        } else if (timeRemaining >= 120 && timeRemaining <= 179) {
            level = level3;
        } else if (timeRemaining >= 60 && timeRemaining <= 119) {
            level = level4;
        } else if (timeRemaining >= 0 && timeRemaining <= 59) {
            level = level5;
        }
    }, 5000);
    launchEnemies();

    setInterval(function(){
        if (isDemo) return;
        $.custom.Server.SaveGameScore(game.score,1, 
            function (res) { // success
                if (res && res.success) {
                    if (res.IsCorrect) {
                        console.log('score saved');              
                    }
                } 
                else { // fail 
                    console.log('score not saved');
                }
        }, function () { // fail
                    console.log('score not saved');           
        });
            
    },5000);


    $('#weapons').on('click', '.weapon', function (event) {
        event.preventDefault();
        $('.weapon').removeClass('selected');
        $(this).addClass('selected');
        weapon = $(this).data('weapon');
        console.log('active weapon: ' + weapon);
        /* Act on the event */
    });

    $('body').on('click', '.monster1', function (event) {
        event.preventDefault();
        if (weapon != 3) {
            // if (game.score >= 1) {
            //    game.score -= 1;
            // }
            return;
        }
        var that = $(this);
        var position = that.data();
        setTimeout(function () {
            that.remove();
        }, 250);

        game.score += 1;
        var boom = '<div class="boom position' + position.position + '"><img src="/Content/images/boom.gif?' + (new Date).getTime() + '"/></div>';
        poof.play();
        $('#stage').append(boom);
        setTimeout(function () {
            $('.boom.position' + position.position).remove();
        }, 700);
    });

    $('body').on('click', '.monster2', function (event) {

        event.preventDefault();
        if (weapon != 1) {
            // if (game.score >= 1) {
            //    game.score -= 1;
            // }
            return;
        }
        var that = $(this);
        var position = that.data();
            game.score += 1;
            var position = $(this).data();
            var boom = '<img class="boom position' + position.position + '" src="/Content/images/boom.gif?' + (new Date).getTime() + '"/>';
            $('#stage').append(boom);
            poof.play();

            setTimeout(function () {
                $('.boom.position' + position.position).remove();
            }, 700);
            setTimeout(function () {
                $('.monster2').remove();
            }, 250)
    });

    $('body').on('click', '.monster4', function (event) {
        event.preventDefault();
        if (weapon != 2) {
            // if (game.score >= 1) {
            //    game.score -= 1;
            // }
            return;
        }
        var that = $(this);
        var position = that.data();
            game.score += 1;
            setTimeout(function () {
                that.remove();
            }, 250)
            var boom = '<img class="boom position' + position.position + '" src="/Content/images/boom.gif?' + (new Date).getTime() + '"/>';
            $('#stage').append(boom);
            poof.play();
            setTimeout(function () {
                $('.boom.position' + position.position).remove();
            }, 700);

    });
    $('body').on('click', '.monster3', function (event) {
        $('#flash').animate({ opacity: 1 }, 100).animate({ opacity: 0 }, 100);
        event.preventDefault();
        var that = $(this);
        var position = that.data();
        setTimeout(function () {
            that.remove();
        }, 250);
        if (game.score >= 1) {
            game.score -= 1;
            if (game.score < 0) {
                game.score = 0;
            }
        }
        var boom = '<div class="boom position' + position.position + '"><img src="/Content/images/boom.gif?' + (new Date).getTime() + '"/></div>';
        poof.play();
        $('#stage').append(boom);
        setTimeout(function () {
            $('.boom.position' + position.position).remove();
        }, 700);
    });

    if (isDemo) return;

    setInterval(function () {
        $.custom.Server.SaveGameTime(timeRemaining,
            function (res) { // success
                if (res && res.success) {
                    console.log('Playtime updated');
                }
                else { // fail
                    console.log('Playtime not updated!');

                }
            }, function () { // fail

            });
    }, 5000);

});
