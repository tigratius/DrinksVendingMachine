
function AdminManager(token) {

    var tokenId = token;

    this.changeImage = function () {
        var id = "uploadFile";
       this.uploadImage(id, function (response) {
            var url = '/Admin/ChangeDrinkImage?token=' + tokenId;
            var id = $("#hiddenImg").val();
            var data = { id: id, filename: response.filename };
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                success: function (response) {
                    if (response != null) {
                        $(`#img_${response.id}`).attr("src", `/Content/Images/${response.filename}`);
                    }
                },
                error: processErrorStd
            });
        });
    };

    this.uploadImage = function (id, callback) {
        var files = document.getElementById(id).files;
        if (files.length > 0) {
            if (window.FormData !== undefined) {
                var data = new FormData();
                var file = files[0];
                data.append('file', file);

                $.ajax({
                    type: "POST",
                    url: '/Admin/Upload?token=' + tokenId,
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (response) {
                        if (callback) callback(response);
                    },
                    error: processErrorStd
                });
            } else {
                alert("Браузер не поддерживает загрузку файлов HTML5!");
            }
        }
    };

    this.addDrink = function () {
        var id = "uploadImageFile";
        this.uploadImage(id, function (response) {
            var url = '/Admin/AddDrink?token=' + tokenId;
            var name = $("#name").val();
            var cost = $("#cost").val();
            var count = $("#count").val();

            if (cost <= 0 || count < 0 || count === "") {
                alert("Ошибка! Проверьте данные.");
                return;
            }

            var data = { name: name, cost: cost, count: count, img: response.filename };
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                success: function (response) {
                    if (response != null) {
                        var markup =
                            `<tr id='row_${response.id}'><td><p><img id='img_${response.id}' src='/Content/Images/${response.img}' style='width: 80px; height: 100px; top: 15px; position: relative' onclick="imageClick('${response.id}')" />
                            </p></td><td><p><input id='dn_${response.id}' type='text' value='${response.name}' onchange="adminManager.changeDrinkName('${response.id}')" />
                            </p></td><td><p><input id='dcst_${response.id}' value='${response.cost}' onchange="adminManager.changeDrinkCost('${response.id}')" />
                            </p></td><td><p><input id='dcnt_${response.id}' type='number' value='${response.count}' onchange="adminManager.changeDrinkCount('${response.id}')" />
                            </p></td><td><p><input type='button' value='Удалить' onclick="adminManager.removeDrink('${response.id}')" /></p></td>`;
                        $("#drink-table tbody").append(markup);
                    }
                },
                error: processErrorStd
            });
        });
    };

    this.removeDrink = function (id) {
        var url = '/Admin/RemoveDrink?token=' + tokenId;
        var data = { id: id };
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function (response) {
                if (response != null) {
                    $(`#row_${response.id}`).remove();
                }
            },
            error: processErrorStd
        });
    };

    this.importDrinks = function (id) {
        var url = '/Admin/Import?token=' + tokenId;
        var data = { id: id };
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function (response) {
                if (response != null) {
                    console.log(response);
                }
            },
            error: processErrorStd
        });
    };

    this.changeBlocking = function (id) {
        var url = '/Admin/ChangeBlocking';
        var isBlocking = $(`#bl_${id}`).prop('checked');
        var data = { id: id, isBlocking: isBlocking };
        makePostRequestSimple(url, data);
    };

    this.changeCoinCount = function (id) {
        var url = '/Admin/ChangeCoinCount';
        var count = $(`#${id}`).val();

        if (count < 0 || count==="") {
            alert("Количество монет не может быть отрицательным!");
            return;
        }

        var data = { id: id, count: count };
        makePostRequestSimple(url, data);
    };

    this.changeDrinkCount = function (id) {

        var url = '/Admin/ChangeDrinkCount';
        var count = $(`#dcnt_${id}`).val();

        if (count < 0 || count==="") {
            alert("Количество напитков не может быть отрицательным!");
            return;
        }

        var data = { id: id, count: count };
        makePostRequestSimple(url, data);
    };

    this.changeDrinkName = function (id) {
        var url = '/Admin/ChangeDrinkName';
        var name = $(`#dn_${id}`).val();
        var data = { id: id, name: name };
        makePostRequestSimple(url, data);
    };

    this.changeDrinkCost = function (id) {
        var url = '/Admin/ChangeDrinkCost';
        var cost = $(`#dcst_${id}`).val();

        if (cost <= 0) {
            alert("Цена напитков не может быть отрицательным или равным 0!");
            return;
        }

        var data = { id: id, cost: cost };
        makePostRequestSimple(url, data);
    };

    function makePostRequestSimple(url, data) {
        $.ajax({
            type: "POST",
            url: url + "?token=" + tokenId,
            data: data,
            error: processErrorStd
        });
    };

    function processErrorStd(xhr, status, error) {
        console.log("AJAX request error!");
        console.log("  xhr:");
        console.log(xhr);
        console.log("  status=" + status);
        console.log("  error=" + error);
        alert(error);
    };
};

imageClick = function (id) {
    $("#hiddenImg").val(id);
    $("#uploadFile").click();
};

