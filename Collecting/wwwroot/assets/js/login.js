var tokenKey = "accessToken";
// Настройка работы кнопки авторизации
$('#signin').on('click', function () {
    getTokenAsync();
    console.log(sessionStorage.getItem(tokenKey));
});

// отпавка запроса к контроллеру AccountController для получения токена
async function getTokenAsync() {
    let user = {
        userEmail: document.getElementById("login-email").value,
        password: document.getElementById("login-password").value,
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
    }
};

function logout() {
    sessionStorage.removeItem(tokenKey);
    console.log(sessionStorage.getItem(tokenKey));
    alert("Птерадактиль");
    window.location.href = "login.html";
}