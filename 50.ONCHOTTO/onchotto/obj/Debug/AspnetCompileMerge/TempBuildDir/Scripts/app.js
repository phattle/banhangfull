function cBack() {
    return false;
}

jQuery(document).ready(function ($) {
    $(".kkcountdown").kkcountdown({
        dayText: ' ngày ',
        daysText: ' ngày ',
        hoursText: ':',
        minutesText: ':',
        secondsText: '',
        displayZeroDays: true,
        callback: cBack,
        rusNumbers: false
    });

    //fix top
    $(window).scroll(function () {
        if ($(window).width() < 768) {
            return false;
        }

        if ($(window).scrollTop() > $('#mainNavPost').offset().top) {
            if (! $('#mainNav').hasClass('navbar-fixed-top')) {
                $('#mainNav').addClass('navbar-fixed-top');
            }
            if ($("#mainNav .category-menu").hasClass('allwayshow')) {
                $("#mainNav .category-menu").removeClass('allwayshow').addClass('allwayshowFixtop');
            }
        } else {
            if ($('#mainNav').hasClass('navbar-fixed-top')) {
                $('#mainNav').removeClass('navbar-fixed-top');
            }

            if ($("#mainNav .category-menu").hasClass('allwayshowFixtop')) {
                $("#mainNav .category-menu").removeClass('allwayshowFixtop').addClass('allwayshow');
            }
        }
    });
    
    //Đặt sau 3s bật popup nếu có
    setTimeout(function(){
        $('.bannerPopup').popup({
            transition: 'all 0.3s',
            scrolllock: true, // optional
            autoopen: true
        });
    }, 3000);
    
    $('.select2').select2();

    setNavigation();
});


function setNavigation() {
    var path = window.location.pathname;
    path = path.replace(/\/$/, "");
    path = decodeURIComponent(path);

    $(".nav a").each(function () {
        var href = $(this).attr('href');
        if (path.substring(0, href.length) === href) {
            $(this).closest('li').addClass('active');
        }
    });
}