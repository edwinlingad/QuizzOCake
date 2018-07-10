
interface IEd {
    initializeImageFill: () => void;
    initializeSmoothScrolling: () => void;
}

var ED: IEd = (function () {
    var initializeImageFill = function () {
        $(window).on('load resize', function () {
            $('.ed-img-fill-screen').css('height', $(window).innerHeight());
        });
    }

    function initializeSmoothScrolling() {
        $('.nav a, .down-button-div a, #goto-top-button-div a').bind('click', function () {
            $('html, body').stop().animate({
                scrollTop: $($(this).attr('href')).offset().top - 75
            }, 1500, 'easeInOutExpo');
            event.preventDefault();
        });
    }

    return {
        initializeImageFill: initializeImageFill,
        initializeSmoothScrolling: initializeSmoothScrolling
    }
})();