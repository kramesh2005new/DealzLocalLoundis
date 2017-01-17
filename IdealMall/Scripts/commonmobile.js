/* for IE7 and IE8 Browsers */
document.createElement('header');
document.createElement('footer');
document.createElement('section');
document.createElement('aside');
document.createElement('nav');
document.createElement('article');


$(function () {
    $("header").on("click", ".menubg", function () {
        $("#menuWrapper").addClass("active");
    });
    $("#menuWrapper").on("click", ".close", function () {
        $("#menuWrapper").removeClass("active");
    });


    //Scroll to top starts
    if ($('#back-to-top').length) {
        var scrollTrigger = 100, // px
            backToTop = function () {
                var scrollTop = $(window).scrollTop();
                if (scrollTop > scrollTrigger) {
                    $('#back-to-top').addClass('show');
                } else {
                    $('#back-to-top').removeClass('show');
                }
            };
        backToTop();
        $(window).on('scroll', function () {
            backToTop();
        });
        $('#back-to-top').on('click', function (e) {
            e.preventDefault();
            $('html,body').animate({
                scrollTop: 0
            }, 700);
        });
    }
    //Scroll to top ends
});



