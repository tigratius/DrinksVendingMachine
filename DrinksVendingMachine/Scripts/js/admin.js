
function AdminManager() {

    var tokenId;
    const _this = this;

    this.init = function (token) {
        tokenId = token;
    }

    this.changeImage = function () {
        const _id = "UploadFile";
        this.uploadImage(_id, function (response) {
            const url = `/Admin/ChangeDrinkImage?token=${tokenId}`;
            const id = $("#HiddenImg").val();
            const data = { id: id, path: response.path };
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                success: function (response) {
                    if (response != null) {
                        $(`#img_${response.id}`).attr("src", `${response.path}`);
                    }
                },
                error: processErrorStd
            });
        });
    };

    this.uploadImage = function (id, callback) {
        const url = `/Admin/Upload?token=${tokenId}`;
        const files = document.getElementById(id).files;
        if (files.length > 0) {
            if (window.FormData !== undefined) {
                const data = new FormData();
                const file = files[0];
                data.append("file", file);

                $.ajax({
                    type: "POST",
                    url: url,
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (response) {
                        if (response != null) {
                            if (response.success) {
                                if (callback) callback(response);
                            } else {
                                alert(response.message);
                            }
                        }
                    },
                    error: processErrorStd
                });
            } else {
                alert("Браузер не поддерживает загрузку файлов HTML5!");
            }
        }
    };

    this.addDrink = function () {
        const id = "UploadImageFile";

        this.uploadImage(id, function (response) {
            const url = `/Admin/AddDrink?token=${tokenId}`;
            const name = $("#Name").val();
            const cost = $("#Cost").val();
            const count = $("#Count").val();

            if (!validateFieldCountAndCost(count, cost)) {
                alert("Ошибка! Проверьте данные.");
                return;
            }

            const data = { name: name, cost: cost, count: count, imgPath: response.path };
            $.ajax({
                type: "POST",
                url: url,
                data: data,
                success: function (response) {
                    if (response != null) {
                        if (response.success) {
                            const markup = `<tr id='row_${response.id}'><td><img id='img_${response.id}' src='${
                                response.path
                            }' class="img img-hover img-thumbnail" onclick="imageClick('${
                                response.id}')" />
                            </td><td><input id='dn_${response.id}' type='text' value='${response.name
                            }' class="input-sm" onchange="adminManager.changeDrinkName('${response.id}')" />
                            </td><td><input id='dcst_${response.id}' value='${response.cost
                            }' class="input-sm" onchange="adminManager.changeDrinkCost('${response.id}')" />
                            </td><td><input id='dcnt_${response.id}' type='number' value='${response.count
                            }' class="input-sm" onchange="adminManager.changeDrinkCount('${response.id}')" />
                            </td><td><input type='button' value='Удалить' class="btn btn-default" onclick="adminManager.removeDrink('${
                                response.id}')" /></td>`;
                            $("#DrinkTable tbody").append(markup);
                        } else {
                            alert(response.message);
                        }
                    }
                },
                error: processErrorStd
            });
        });
    };

    this.removeDrink = function (id) {
        const url = `/Admin/RemoveDrink?token=${tokenId}`;
        const data = { id: id };
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

    this.importDrinks = function () {
        const id = "ImportDataFile";
        const url = `/Admin/Import?token=${tokenId}`;

        const files = document.getElementById(id).files;
        if (files.length > 0) {
            if (window.FormData !== undefined) {
                const data = new FormData();
                const file = files[0];
                data.append("file", file);

                $("#ShowLoadMsg").show();

                $.ajax({
                    type: "POST",
                    url: url,
                    contentType: false,
                    processData: false,
                    data: data,
                    success: function (response) {
                        if (response != null) {
                            if (response.success) {
                                $(document).ajaxStop(function () { location.reload(true); });
                            } else {
                                alert(response.message);
                            }
                        }
                        $("#ShowLoadMsg").hide();
                    },
                    error: processErrorStd
                });
            } else {
                alert("Браузер не поддерживает загрузку файлов HTML5!");
            }
        }
    };

    this.changeBlocking = function (id) {
        const url = "/Admin/ChangeBlocking";
        const isBlocking = $(`#bl_${id}`).prop("checked");
        const data = { id: id, isBlocking: isBlocking };
        makePostRequestSimple(url, data);
    };

    this.changeCoinCount = function (id) {
        const url = "/Admin/ChangeCoinCount";
        const count = $(`#${id}`).val();

        if (!validateFieldCount(count)) {
            alert("Количество монет не может быть отрицательным!");
            return;
        }

        const data = { id: id, count: count };
        makePostRequestSimple(url, data);
    };

    this.changeDrinkCount = function (id) {

        const url = "/Admin/ChangeDrinkCount";
        const count = $(`#dcnt_${id}`).val();

        if (!validateFieldCount(count)) {
            alert("Количество напитков не может быть отрицательным!");
            return;
        }

        const data = { id: id, count: count };
        makePostRequestSimple(url, data);
    };

    this.changeDrinkName = function (id) {
        const url = "/Admin/ChangeDrinkName";
        const name = $(`#dn_${id}`).val();
        const data = { id: id, name: name };
        makePostRequestSimple(url, data);
    };

    this.changeDrinkCost = function (id) {
        const url = "/Admin/ChangeDrinkCost";
        const cost = $(`#dcst_${id}`).val();

        if (!validateFieldCost(cost)) {
            alert("Цена напитков не может быть отрицательным или равным 0!");
            return;
        }

        const data = { id: id, cost: cost };
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
        console.log(`  status=${status}`);
        console.log(`  error=${error}`);
        alert(error);
    };

    function validateFieldCost(cost) {
        return cost > 0 && cost !== "";
    }

    function validateFieldCount(count) {
        return count >= 0 && count !== "";
    }

    function validateFieldCountAndCost(count, cost) {
        return validateFieldCost(cost) && validateFieldCount(count);
    }


};

imageClick = function (id) {
    $("#HiddenImg").val(id);
    $("#UploadFile").click();
};

inputChangeImport = function () {
    inputChange("#ImportLbl");
    $("#ImportSubmit").removeProp("disabled");
};

inputChange = function (id) {

    const wrapper = $(id);
    const inp = wrapper.find("input");
    const btn = wrapper.find(".button");
    const lbl = wrapper.find("mark");

    const fileApi = (window.File && window.FileReader && window.FileList && window.Blob) ? true : false;

    var fileName;
    if (fileApi && inp[0].files[0]) {
        fileName = inp[0].files[0].name;
    }
    else {
        fileName = "Файл не выбран";
    }

    if (lbl.is(":visible")) {
        lbl.text(fileName);
        btn.text("Выбрать");
    } else
        btn.text(fileName);
}

