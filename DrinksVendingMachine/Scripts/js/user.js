function UserManager() {

    var that = this;

    this.imageClick = function (id, cost, count) {

        var totalSum = parseInt($('#totalSum').text());
        var c = parseInt(cost);
        var cnt = parseInt(count);

        /*if (totalSum < c || cnt < 1) {
            return;
        }*/

        if (!validateConditionToBuy(c, cnt, totalSum)) {
            return;
        }

        var image = $(`#img_${id}`);

        if (!image.hasClass('img-hover')) {
            return;
        } else {
            image.addClass('img-choosen');
            $("#drinkInfo img").removeClass('img-hover');
            $("#button-buy").removeProp('disabled');
            $("#button-cancel").removeProp('disabled');
            $("#drink-id").val(id);
        };
    }

    this.cancel = function () {
        $("#drinkInfo img").removeClass('img-choosen');
        $("#drinkInfo img").addClass('img-hover');
        $("#button-buy").prop('disabled', true);
        $("#button-cancel").prop('disabled', true);
        $("#drink-id").val("");
    }

    this.buy = function () {
        var url = '/User/BuyDrink';
        var id = $("#drink-id").val();
        var data = { id: id };
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function (response) {
                if (response != null) {
                    that.cancel();
                    $("#totalSum").text(0);
                    $("#change").text(response.change);
                    if (response.change > 0) {
                        $("#button-take-money").removeProp('disabled');
                    }

                    if (!response.success) {
                        alert(response.msg);
                    }
                }
            },
            error: processErrorStd
        });
    }

    this.takeMoney = function () {
        var url = '/User/GetChange';
        $.ajax({
            type: "GET",
            url: url,
            success: function () {
                $("#button-take-money").prop('disabled', true);
                $("#change").text(0);
            },
            error: processErrorStd
        });
    }

    this.addCoin = function (id) {
        var url = '/User/AddCoin';
        var data = { id: id };
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function (response) {
                if (response != null) {
                    $("#totalSum").text(response.deposit);
                    $("#button-take-money").prop('disabled', true);
                }
            },
            error: processErrorStd
        });
    };

    function validateConditionToBuy(cost, count, totalSum) {
        return totalSum >= cost && count > 0;
    }

    function processErrorStd (xhr, status, error) {
        console.log("AJAX request error!");
        console.log("  xhr:");
        console.log(xhr);
        console.log("  status=" + status);
        console.log("  error=" + error);
        alert(error);
    };
}