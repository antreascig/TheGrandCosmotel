var isMobile = false; //initiate as false
// device detection
if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent)
    || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4))) isMobile = true;

var isPaused = false;
var timeRemaining = null;
var item = null;
var itemClue = null;
var stage = $('.game').data('stage');
var level = $('.game').data('level');
var cVal = null;
var gameTime = null;
var enteredCode = null;
//var unlockCode = null;
var score = GameScore;
var levelScore = null;
var music = $("#myAudio")[0];
if ($('.game').data('level') == 'level1') {
    levelScore = 70;
} else {
    levelScore = 70;
}

$('#score span').html(GameScore);

//unlockCode = Math.floor(Math.random() * (999 - 100 + 1)) + 100;;

function startTimer(duration, display) {
    var timer = duration, minutes, seconds;

    setInterval(function () {
        if (!isPaused) {
            minutes = parseInt(timer / 60, 10)
            seconds = parseInt(timer % 60, 10);
            minutes = minutes < 10 ? "0" + minutes : minutes;
            seconds = seconds < 10 ? "0" + seconds : seconds;

            display.text(minutes + ":" + seconds);
            if (--timer <= 0) {
                isPaused = true;
                endGame();
                // timer = duration;
            }
            timeRemaining = timer;
        } else {
            timeRemaining = timer;
        }
    }, 1000);
}
var success = $("#mysoundclip")[0];

function endGame() {
    $('.wrapper').fadeOut(2000, function () {
        if (isDemo) {
            window.location.replace("/Games/ActiveExplainer");
        }
        else {
            window.location.replace("/Games/ActiveGameAfter?status=outoftime");
        }
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

// callback that will be run once images are ready 
loader.addCompletionListener(function () {
    var time = null;
    time = RemainingTime;
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
        music.play();
    }

});


$(window).on('resize', function () {
    $('.game').center();
});

$(document).ready(function () {

    loader.start();
    $('.exit').click(function (event) {
        /* Act on the event */
        isPaused = false;
        $('#explainer').hide();
    });
    $('.pause').click(function (event) {
        /* Act on the event */
        isPaused = true;
        $('#explainer').show();
    });
    $('.wrapper').fadeIn(2000);
    $('.codenum').easyAudioEffects({
        ogg: "/Content/music/number.ogg",
        mp3: "/Content/music/number.mp3",
        eventType: "click" // or "click"

    });
    $('.item').easyAudioEffects({
        ogg: "/Content/music/click.ogg",
        mp3: "/Content/music/click.mp3",
        eventType: "click" // or "click"

    });
    $('.close').easyAudioEffects({
        ogg: "/Content/music/close.ogg",
        mp3: "/Content/music/close.mp3",
        eventType: "click" // or "click"

    });
    $('.game').center();

    $('.gamewrap').on('click', '.item', function (event) {
        item = $(this);
        itemClue = item.data('clue');
        event.preventDefault();
        /* Act on the event */
        $('.zoom').empty();
        $('.zoom').append('<img src="/Content/images/cells/' + stage + '/' + level + '/' + itemClue + '.jpg" />');
        $('.popup').fadeIn(500);
    });
    $('body').on('click', '.close', function (event) {
        event.preventDefault();
        $(this).parent().fadeOut(500);
        $('.overlay').fadeOut(500);
        /* Act on the event */
    });
    $('.popup .close').click(function (event) {
        /* Act on the event */
        $('.popup').fadeOut('500', function () {
            $('.zoom').empty();
        });
    });
    $('#lock').click(function (event) {
        /* Act on the event */
        $('.overlay, .unlock').fadeIn(500);
    });
    $('.codenum').click(function (event) {
        /* Act on the event */
        cVal = parseInt($(this).html());
        if (cVal < 9) {
            cVal++;
            $(this).html(cVal);
        } else {
            cVal = 0;
            $(this).html(cVal);
        }
    });
    $('.item').hover(function () {
        $(this).glow({ radius: "8", color: "#fffb3d" });
    }, function () {
        $(this).glow({ disable: true });
    });

    $('#go').click(function (event) {
        /* Act on the event */

        enteredCode = parseInt($('#c1').html() + $('#c2').html() + $('#c3').html());


        if (enteredCode == unlockCode) {
            // alert('woohooo');
            success.play();
            isPaused = true;
            score += levelScore;
            $('.overlay').addClass('front').append('<div id="nextmessage"><span>Μπράβο!</span><br/> Υπάρχουν κι άλλες κλειδαριές που σε περιμένουν!</div>').show(500);
            if (isDemo) {
                endGame();
                return;
            }
            //debugger
            $.custom.Server.SaveGameScore(score, level.substring(5),
                function (res) { // success
                    if (res && res.success) {
                        setTimeout(function () {
                            $('.wrapper').fadeOut(2000, function () {
                                setTimeout(function () {
                                    location.reload();
                                }, 500);
                            });
                        }, 4000);
                    }
                    else { // fail 

                    }
                }, function () { // fail

                });

        } else {
            $(this).toggleClass('wrong');
            setTimeout(function () {
                $('#go').toggleClass('wrong');
            }, 1000);
        }
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