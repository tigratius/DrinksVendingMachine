function UserManager() {

    var _this = this;

    this.init =function(deposit, change) {
        $("#TotalSum").text(deposit);
        $("#Change").text(change);
        if (change > 0 && deposit === 0) {
            $("#ButtonTakeMoney").removeProp("disabled");
        }
    }

    this.cancel = function () {
        $("#DrinkInfo img").removeClass("img-choosen");
        $("#DrinkInfo img").not(".img-empty").addClass("img-hover");

        $("#ButtonBuy").prop("disabled", true);
        $("#ButtonCancel").prop("disabled", true);
        $("#DrinkId").val("");
    }

    this.buy = function () {
        const url = "/User/BuyDrink";
        const id = $("#DrinkId").val();
        const data = { id: id };
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function (response) {
                if (response != null) {
                    $("#TotalSum").text(0);
                    $("#Change").text(response.change);
                    if (response.change > 0) {
                        $("#ButtonTakeMoney").removeProp("disabled");
                    }
                    if (response.count < 1) {
                        $(`#img_${response.id}`).addClass("img-empty");
                    }
                    if (!response.success) {
                        alert(response.msg);
                    }
                    _this.cancel();
                }
            },
            error: processErrorStd
        });
    }

    this.takeMoney = function () {
        const url = "/User/GetChange";
        $.ajax({
            type: "GET",
            url: url,
            success: function () {
                $("#ButtonTakeMoney").prop("disabled", true);
                $("#Change").text(0);
            },
            error: processErrorStd
        });
    }

    this.addCoin = function (id) {
        const url = "/User/AddCoin";
        const data = { id: id };
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function (response) {
                if (response != null) {
                    $("#TotalSum").text(response.deposit);
                    $("#ButtonTakeMoney").prop("disabled", true);
                }
            },
            error: processErrorStd
        });
    };

    this.IsCanBuy = function (id) {

        if ($(`#img_${id}`).hasClass("img-empty"))
            return;

        const url = "/User/IsCanBuy";
        const data = { id: id };
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function (response) {
                if (response != null) {
                    if (response.success) {
                        const image = $(`#img_${id}`);
                        if (!image.hasClass("img-hover")) {
                            return;
                        } else {
                            image.addClass("img-choosen");
                            $("#DrinkInfo img").removeClass("img-hover");
                            $("#ButtonBuy").removeProp("disabled");
                            $("#ButtonCancel").removeProp("disabled");
                            $("#DrinkId").val(id);
                        };
                    } else {
                        alert(response.message);
                    }
                }
            },
            error: processErrorStd
        });
    }

    function processErrorStd (xhr, status, error) {
        console.log("AJAX request error!");
        console.log("  xhr:");
        console.log(xhr);
        console.log(`  status=${status}`);
        console.log(`  error=${error}`);
        alert(error);
    };
}