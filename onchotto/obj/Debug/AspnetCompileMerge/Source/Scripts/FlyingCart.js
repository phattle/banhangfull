

$.ajaxSetup({ cache: false });

function addToCart() {

    pid = $("#addToCart").attr("data-id");
    var ty = $("#addToCart").closest('.product').find('#' + pid);

    var img = $("#" + pid);
    soluong = $('#soluong').val();
    if (soluong == 'undefined' || soluong == null) {
        soluong = 1;
    }
    $.ajax({
        url: "/Cart/Add",
        data: { id: pid, soluong: soluong },
        success: function (response) {
            $("#nn-cart-count").text($.number(response.Count));
            $(".nn-cart-total").text($.number(response.Total) + "đ");
        }

    }).done(function (response) {
        $("#cart-item").load("/Cart/PartialCart")

        flyToElement($(ty), $('#basketid'));
        $("html, body").animate({ scrollTop: 0 }, 800);

        $('#basketid').trigger("click");

        setTimeout(function () {
            $('#basketid').trigger("click");
        }, 8000);

    }).fail(function () {
        alert("fail");
    });

    return false;
}

$(document).ready(function () {

    $("#cart-item").load("/Cart/PartialCart");
    // $('.addProduct').click(function ()

    $(document).delegate(".removeProduct", "click", function (event) {
        event.preventDefault();

        pid = $(this).attr("data-id");

        var img = $("#" + pid);

        flyFromElement($(img), $('#basketid'));
        return false;

    });



});

function flyToElement(flyer, flyingTo, callBack /*callback is optional*/) {

    var $func = $(this);

    var divider = 3;

    var flyerClone = $(flyer).clone();

    $(flyerClone).css({
        position: 'absolute',
        top: $(flyer).offset().top + "px",
        left: $(flyer).offset().left + "px",
        opacity: 1,
        'z-index': 1000
    });

    $('body').append($(flyerClone));

    var gotoX = $(flyingTo).offset().left + ($(flyingTo).width() / 2) - ($(flyer).width() / divider) / 2;
    var gotoY = $(flyingTo).offset().top + ($(flyingTo).height() / 2) - ($(flyer).height() / divider) / 2;

    $(flyerClone).animate({
        opacity: 0.4,
        left: gotoX,
        top: gotoY,
        width: $(flyer).width() / divider,
        height: $(flyer).height() / divider
    }, 700,
    function () {
        $(flyingTo).fadeOut('fast', function () {
            $(flyingTo).fadeIn('fast', function () {
                $(flyerClone).fadeOut('fast', function () {
                    $(flyerClone).remove();
                    if (callBack != null) {
                        callBack.apply($func);
                    }
                });
            });
        });
    });

    return false;
}

function flyFromElement(flyer, flyingTo, callBack /*callback is optional*/) {
    var $func = $(this);

    var divider = 3;

    var beginAtX = $(flyingTo).offset().left + ($(flyingTo).width() / 2) - ($(flyer).width() / divider) / 2;
    var beginAtY = $(flyingTo).offset().top + ($(flyingTo).width() / 2) - ($(flyer).height() / divider) / 2;

    var gotoX = $(flyer).offset().left;
    var gotoY = $(flyer).offset().top;

    var flyerClone = $(flyer).clone();

    $(flyerClone).css({
        position: 'absolute',
        top: beginAtY + "px",
        left: beginAtX + "px",
        opacity: 0.4,
        'z-index': 1000,
        width: $(flyer).width() / divider,
        height: $(flyer).height() / divider
    });
    $('body').append($(flyerClone));

    $(flyerClone).animate({
        opacity: 1,
        left: gotoX,
        top: gotoY,
        width: $(flyer).width(),
        height: $(flyer).height()
    }, 700,
    function () {
        $(flyerClone).remove();
        $(flyer).fadeOut('fast', function () {
            $(flyer).fadeIn('fast', function () {
                if (callBack != null) {
                    callBack.apply($func);
                }
            });
        });
    });

    return false;
}

$(function () {
    // Xóa khỏi giỏ
    //$(".remove-from-cart").click(function ()

    $(document).delegate(".remove-from-cart, .removeProduct", "click", function () {
        pid = $(this).attr("data-id");
        tr = $(this).parents("tr");// tim <tr> chua <img> bi click
        $.ajax({
            url: "/Cart/Remove",
            data: { id: pid },
            success: function (response) {
                $("#nn-cart-count").html(response.Count);
                $(".nn-cart-total").text($.number(response.Total) + "đ");
                $("#cartTotal").val(response.Total);

                $("#cart-item").load("/Cart/PartialCart");
                tr.hide(500);
            },
            complete: function () {
                updateOrderSummary();
            }
        });
    });
    // Cập nhật số lượng trong đơn hàng đặt Order checkout

    $(document).delegate(".quantity, .spquantity", "change", function () {
        pid = $(this).attr("data-id");
        qty = $(this).val();
        //$("#"+pid+"-ss").val(qty);
        $.ajax({
            url: "/Cart/Update",
            data: { id: pid, quantity: qty },
            success: function (response) {
                $("#nn-cart-count").html(response.Count);
                $("#" + pid + "-ss").attr("value", response.quantity);
                $("#Amount-" + pid).html($.number(response.Amount) + "đ");                 
                $("#Weightnet-" + pid).html(response.Weightnet.toFixed(2) + " Lbs");
                $("#FederalTaxUS-" + pid).html($.number(response.FederalTaxUS) + "đ");
                $("#TaxExportVN-" + pid).html($.number(response.TaxExportVN)+ "đ");
                $(".nn-cart-total").html($.number(response.Total) + "đ");
                $("#cartTotal").val(response.Total);
                $(".federalTaxUS-cart-total").html($.number(response.FederalTaxUS.toFixed(2)) + "đ");
                $(".TaxExportVN-cart-total").html($.number(response.TaxExportVN.toFixed(2)) + "đ");

                $(".ShippingInLandUS-cart-total").html($.number(response.ShippingInLandUS.toFixed(2)) + "đ");
                $(".AFFeeVN-cart-total").html($.number(response.AFFeeVN.toFixed(2)) + "đ");
                $(".TECSServicesFeeVN-cart-total").html($.number(response.TECSServicesFeeVN.toFixed(2)) + "đ");
                $("#transactionFee").html($.number(response.TransactionFeeVN.toFixed(2)) + "đ");
                //$("#" + pid).html("$" + response.quantity);
                $("#cart-item").load("/Cart/PartialCart");

            },
            complete: function () {
                updateOrderSummary();
            }
        });
    });

});
