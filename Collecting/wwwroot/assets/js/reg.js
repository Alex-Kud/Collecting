var tokenKey = "accessToken";
// Настройка работы кнопки авторизации
$('#reg').on('click', function () {
    console.log("reg click");
    if ($('#password').val() != $('#password').val()) {
        console.log("Пароли не совпали!");
        return;
    }
    registration();
});

// Запрос на регистрацию аккаунта
async function registration() {
    let user = {
        id: 0,
        name: $('#name').val(),
        surname: $('#surname').val(),
        country: $('#country').val(),
        address: $('#address').val(),
        index: $('#index').val(),
        email: $('#email').val(),
        password: $('#password').val(),
        role: 0,
        cartId: 0
    }
    // отправляет запрос и получаем ответ
    const response = await fetch("../api/Users/Create", {
        method: "POST",
        headers: { "Content-type": "application/json" },
        body: JSON.stringify(user)
    });
    // получаем данные 
    let data = await response.json();

    // если запрос прошел нормально
    if (response.ok === true) {
        getTokenAsync()
    }
    else {
        // если произошла ошибка, из errorText получаем текст ошибки
        console.log("Error: ", response.status);
        let messange = ["Oops. It seems that you have entered incorrect data"];
        getNotification(messange);
    }
}

// отпавка запроса к контроллеру AccountController для получения токена
async function getTokenAsync() {
    let user = {
        userEmail: $('#email').val(),
        password: $('#password').val(),
    }

    // отправляет запрос и получаем ответ
    const response = await fetch("../api/Auth/Auth", {
        method: "POST",
        headers: { "Content-type": "application/json" },
        body: JSON.stringify(user)
    });
    // получаем данные 
    let data = await response.json();

    // если запрос прошел нормально
    if (response.ok === true) {
        var tokenKey = "accessToken";
        // сохраняем в хранилище sessionStorage токен доступа
        sessionStorage.setItem(tokenKey, data.token);

        console.log(sessionStorage.getItem(tokenKey));
        $("#myaccount").attr("href", "account.html");
        $("#account").attr("href", "javascript:my_function()");
        $("#account").html("Logout");
        window.location.href = "account.html";
    }
    else {
        // если произошла ошибка, из errorText получаем текст ошибки
        console.log("Error: ", response.status);
        let messange = ["Oops. It seems that you have entered incorrect data"];
        getNotification(messange);
    }
};