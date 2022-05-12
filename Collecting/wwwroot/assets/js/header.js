const token = sessionStorage.getItem("accessToken");
if (token != null) {
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
            console.log(result.quantity);
            $("#cartQuantity").html(result.quantity);
        }
    });
}
else {
    $("#miniСart").css("display", "none");
}

function logout() {
    sessionStorage.removeItem("accessToken");
    console.log(sessionStorage.getItem("accessToken"));
    window.location.href = "login.html";
}