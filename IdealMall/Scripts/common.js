/* for IE7 and IE8 Browsers */
document.createElement('header');
document.createElement('footer');
document.createElement('section');
document.createElement('aside');
document.createElement('nav');
document.createElement('article');




$(function () {
    var pgurl = window.location.href.substr(window.location.href.lastIndexOf("/") + 1);
    $(".mainnav ul li a").each(function () {
        //alert(pgurl);
        if ($(this).attr("href") == pgurl || $(this).attr("href") == '')
            $(this).addClass("active");

    })
    $(".mainnav ul li a").click(function () {
        $(".mainnav ul li a").removeClass('active');
        $(this).addClass('active');
        // alert(location.href.toLowerCase());
    });

    // var alert=(location.href.toLowerCase());

    /*$('.mainnav ul li a').each(function () {
        if (str.indexof(this.href.toLowerCase()) > -1) {
            $('.mainnav ul li a').removeClass("active");
            $(this).addClass("active");
        }
    });*/

    //$('.mainnav li a').click(function () {
    //    (this).addClass('active');
    //    $('.mainnav li a').removeClass('active');
    //});

    
    $(".leftnav ul li a").each(function () {
        $(this).hover(function () {
            $(this).parent().parent().prev().addClass('active');
        },
            function () {
                $('.leftnav ul li a').removeClass('active');
            });
    });
    $(".magnifierbtn").colorbox({ transition: "none" });

    //$(".companypopup").colorbox({
    //    inline: true,
    //    width: "640px",
    //    escKey: false,
    //      overlayClose: false
    //});
    //$('#companies .companyname:last-child, .product-block .productbox:last-child').addClass('marginnone');


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

    $('.product-contrn .product-block .productbox:last-child').addClass('last');
    $("ul#tabs li").click(function (e) {
        if (!$(this).hasClass("active")) {
            var tabNum = $(this).index();
            var nthChild = tabNum + 1;
            $("ul#tabs li.active").removeClass("active");
            $(this).addClass("active");
            $("ul#tab li.active").removeClass("active");
            $("ul#tab li:nth-child(" + nthChild + ")").addClass("active");
        }
    });


    /*$("#banner_fade li").hover(function(){
    
  $(".bjqs-next, .bjqs-prev").css("display","block");
  },function(){
  $(".bjqs-next, .bjqs-prev").css("display","none");
});*/
    /*Slider ends*/




    $(".leftnav ul li a").each(function () {
        $(this).hover(function () {
            $(this).parent().parent().prev().addClass('active');
        },
            function () {
                $('.leftnav ul li a').removeClass('active');
            });
    });

    $(".companypopup").colorbox({
        inline: true,
        width: "640px",
        escKey: false,
        overlayClose: false
    });
    $('#companies .companyname:last-child').addClass('marginnone');


});


function bigdiv() {

    var bigbrother = -1;
    $('.productbox').each(function () {
        bigbrother = bigbrother > $('.productbox').height() ? bigbrother : $('.productbox').height();
    });

    $('.productbox').each(function () {
        $('.productbox').height(bigbrother);
    });
}
function charcountspan() {
    var lengthchar = $('#spnproduct').text().length;
    if (lengthchar > 41) {
        $("#spnvolume").css('display', 'inline');
    }
    else {
        $("#spnvolume").css('display', 'display:block');
    }

    var lengthchar1 = $('#spnproduct1').text().length;
    if (lengthchar1 > 41) {
        $("#spnvolume1").css('display', 'inline');
    }
    else {
        $("#spnvolume1").css('display', 'display:block');
    }

    var lengthchar2 = $('#spnproduct2').text().length;
    if (lengthchar2 > 41) {
        $("#spnvolume2").css('display', 'inline');
    }
    else {
        $("#spnvolume2").css('display', 'display:block');
    }
}