//$(document).delegate(".removeProduct", "click", function () {

//    pid = $(this).attr("data-id-remove");

//    $.ajax({
//        url: "/Cart/Remove",
//        data: { id: pid },
//        success: function (response) {
//            $("#nn-cart-count").html(response.Count);
//            $(".nn-cart-total").html(response.Total);
//            $("#" + pid + " sp").hide(500);
//            $("#cart-item").load("/Cart/PartialCart");
//        }
//    });

//});

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
                $(".nn-cart-total").html(response.Total);
                $("#cart-item").load("/Cart/PartialCart");
                tr.hide(500);
            }
        });
    });
    // Cập nhật số lượng

    $(document).delegate(".quantity, .spquantity", "keyup", function () {
        pid = $(this).attr("data-id");
        qty = $(this).val();
        //$("#"+pid+"-ss").val(qty);
        $.ajax({
            url: "/Cart/Update",
            data: { id: pid, quantity: qty },
            success: function (response) {
                $("#nn-cart-count").html(response.Count);
                $(".nn-cart-total").html(response.Total);
                $("#" + pid).html("$" + response.Amount);
                $("#" + pid + "-ss").attr("value", response.quantity);

                //$("#" + pid).html("$" + response.quantity);

                $("#cart-item").load("/Cart/PartialCart");

            }
        });
    });

});