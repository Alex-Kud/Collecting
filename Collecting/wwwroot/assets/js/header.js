const token = sessionStorage.getItem("accessToken");

// Отправка запроса на сервер
$.ajax({
    type: 'GET',
    dataType: 'json',
    headers: { "Authorization": "Bearer " + token },
    url: "../api/Auth/GetResult",
    // После получения ответа сервера
    success: function (result) {
        if (result == "Всё ок") {
            $("#myaccount").attr("href", "account.html");
            $("#account").attr("href", "javascript:logout()");
            $("#account").html("Logout");

            // Отправка запроса на сервер
            $.ajax({
                type: 'GET',
                dataType: 'json',
                headers: { "Authorization": "Bearer " + token },
                url: "../api/Carts/QuantityUser",
                // После получения ответа сервера
                success: function (result) {
                    $("#cartQuantity").html(result.quantity);
                }
            });
        }
    },
    error: function () {
        $("#miniСart").css("display", "none");
    }
});

function logout() {
    sessionStorage.removeItem("accessToken");
    window.location.href = "login.html";
}

// Вывод уведомлений
function getNotification(text) {
    for (let i = 0; i < text.length; ++i) {
        $(`#notification`).html("")
        $(`#notification`).append(`<span>${text[i]}</span>`);
    }
    $('#notification').addClass('show');
    $('#notification')
    setTimeout(() => {
        $('#notification').removeClass('show');
    }, 7000);
}