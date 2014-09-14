var hidden = false;
var hidden_text = "Изменить имя";
var shown_text = "Спрятать";

onload = function () {
    $("#change_name").hide();
    hidden = true;

    

    jQuery('#search-box').on('input propertychange paste', function () {
        if (this.value !== "") {
            document.getElementById("results").innerHTML = "<img src=\"https://www.bestwestern.com/img/ajaxSpinner.gif\" />";
            $.get("/Account/Search",
                "query=" + this.value,
                function (data) {
                    document.getElementById("results").innerHTML = data;
                    $(".search-elem").on('click', function (txt) {
                        location.href = 'Group/AddUser?group_id=' + gr_id + '&user_id=' + txt.currentTarget.id;
                    });
                }, "html").fail(function () {
                    document.getElementById("results").innerHTML = "";
                });
        }
        else {
            document.getElementById("results").innerHTML = "";
        }
    });   
}

var hide_result = function () {
    $("#results").hide();
}

var show_result = function () {
    $("#results").show();
}

var triger = function () {
    var container = $("#change_name");
    if (hidden) {
        container.show();
        $("#triger").text(shown_text);
        hidden = false;
    }
    else {
        container.hide();
        $("#triger").text(hidden_text);
        hidden = true;
    }
}

