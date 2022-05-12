init();

function init() {
    console.log("Скрипт запустился");
    /*// Создаём объект класса XMLHttpRequest
    const request = new XMLHttpRequest();
    const url = "\WeatherForecast";
    request.open('GET', url);
    // Указываем заголовки для сервера, говорим что тип данных, - контент который мы хотим получить должен быть не закодирован.
    request.setRequestHeader('Content-Type', 'application/x-www-form-url');
    // Здесь мы получаем ответ от сервера на запрос, лучше сказать ждем ответ от сервера
    request.addEventListener("readystatechange", () => {
        // request.readyState - возвращает текущее состояние объекта XHR(XMLHttpRequest) объекта,
        // бывает 4 состояния 4-е состояние запроса - операция полностью завершена, пришел ответ от сервера,
        // вот то что нам нужно request.status это статус ответа,
        // нам нужен код 200 это нормальный ответ сервера, 401 файл не найден, 500 сервер дал ошибку и прочее...
        if (request.readyState === 4 && request.status === 200) {
            console.log("Всё хорошо");
            console.log(JSON.parse(request.responseText));
        }
        else {
            console.log("Сервер шалит");
        }
    });
    request.send(); // Выполняем запрос*/ //document.getElementById('mini-cart-dropdown').innerHTML;


    /*
    // Создаём объект класса XMLHttpRequest
    const request = new XMLHttpRequest();
    const url = "../Carts/Quantity/1";
    request.open('GET', url);
    // Указываем заголовки для сервера, говорим что тип данных, - контент который мы хотим получить должен быть не закодирован.
    request.setRequestHeader('Content-Type', 'application/x-www-form-url');
    // Здесь мы получаем ответ от сервера на запрос, лучше сказать ждем ответ от сервера
    request.addEventListener("readystatechange", () => {
        // request.readyState - возвращает текущее состояние объекта XHR(XMLHttpRequest) объекта,
        // бывает 4 состояния 4-е состояние запроса - операция полностью завершена, пришел ответ от сервера,
        // вот то что нам нужно request.status это статус ответа,
        // нам нужен код 200 это нормальный ответ сервера, 401 файл не найден, 500 сервер дал ошибку и прочее...
        if (request.readyState === 4 && request.status === 200) {
            console.log("Всё хорошо");
            console.log(JSON.parse(request.responseText));
        }
        else {
            console.log("Сервер шалит");
        }
    });
    request.send(); // Выполняем запрос*/

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