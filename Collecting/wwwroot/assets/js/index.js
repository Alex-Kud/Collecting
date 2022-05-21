init();

function init() {
    const token = sessionStorage.getItem("accessToken");
    if (token != null) {
        // Отправка запроса на сервер
        $.ajax({
            type: 'GET',
            dataType: 'json',
            headers: { "Authorization": "Bearer " + token },
            url: "../api/Users/GetUser",
            // После получения ответа сервера
            success: function (user) {
                console.log(user);
            }
        });
    }
}