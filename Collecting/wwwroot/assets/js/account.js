﻿let currentUser;
// Получение данных о текущем пользователе
$.ajax({
    type: 'GET',
    dataType: 'json',
    headers: { "Authorization": "Bearer " + sessionStorage.getItem("accessToken") },
    url: "../api/Users/GetUser",
    // После получения ответа сервера
    success: function (user) {
        $('#userName').val(user.name);
        $('#userSurname').val(user.surname);
        $('#userEmail').val(user.email);
        $('#userCountry').val(user.country);
        $('#userIndex').val(user.index);
        $('#userAddress').val(user.address);
        currentUser = user;

        let currentFeature = 0;
        let content = $('<li id="0feature" class="active"><a>Personal data<span class="fa fa-angle-double-right"></span></a></li>');
        content.on('click', function () {
            $(`#${(currentFeature)}feature`).removeClass('active');
            currentFeature = 0;
            $(`#${(currentFeature)}feature`).addClass('active');
            $('#personalData').css("display", "block");
            $('#myOrders').css("display", "none");
            $('#addSricker').css("display", "none");
            $('#allUsers').css("display", "none");
            $('#allOrders').css("display", "none");
        });
        $('#features').append(content);

        // Обычный пользователь
        if (user.role == 1) {
            let content1 = $(`<li id="1feature" class=""><a>My orders<span class="fa fa-angle-double-right"></span></a></li>`);
            content1.on('click', function () {
                $(`#${(currentFeature)}feature`).removeClass('active');
                currentFeature = 1;
                $(`#${(currentFeature)}feature`).addClass('active');
                $('#personalData').css("display", "none");
                $('#myOrders').css("display", "block");
                $('#addSricker').css("display", "none");
                $('#allUsers').css("display", "none");
                $('#allOrders').css("display", "none");
                getHistory();
            });
            $('#features').append(content1);
        }

        // Коллекционер-модератор
        if (user.role == 2) {
            let content1 = $(`<li id="1feature" class=""><a>Add a sticker<span class="fa fa-angle-double-right"></span></a></li>`);
            content1.on('click', function () {
                $(`#${(currentFeature)}feature`).removeClass('active');
                currentFeature = 1;
                $(`#${(currentFeature)}feature`).addClass('active');
                $('#personalData').css("display", "none");
                $('#myOrders').css("display", "none");
                $('#addSricker').css("display", "block");
                $('#allUsers').css("display", "none");
                $('#allOrders').css("display", "none");
                addSticker();
            });
            $('#features').append(content1);
            /*
            let content2 = $(`<li id="2feature" class=""><a>Current orders<span class="fa fa-angle-double-right"></span></a></li>`);
            content2.on('click', function () {
                $(`#${(currentFeature)}feature`).removeClass('active');
                currentFeature = 2;
                $(`#${(currentFeature)}feature`).addClass('active');
                $('#personalData').css("display", "none");
            });
            $('#features').append(content2);
            */
        }

        // Администратор
        if (user.role == 3) {
            let content1 = $(`<li id="1feature" class=""><a>User settings<span class="fa fa-angle-double-right"></span></a></li>`);
            content1.on('click', function () {
                $(`#${(currentFeature)}feature`).removeClass('active');
                currentFeature = 1;
                $(`#${(currentFeature)}feature`).addClass('active');
                $('#personalData').css("display", "none");
                $('#myOrders').css("display", "none");
                $('#addSricker').css("display", "none");
                $('#allUsers').css("display", "block");
                $('#allOrders').css("display", "none");
                getUsers();
            });
            $('#features').append(content1);
            
            let content2 = $(`<li id="2feature" class=""><a>Order settings<span class="fa fa-angle-double-right"></span></a></li>`);
            content2.on('click', function () {
                $(`#${(currentFeature)}feature`).removeClass('active');
                currentFeature = 2;
                $(`#${(currentFeature)}feature`).addClass('active');
                $('#personalData').css("display", "none");
                $('#myOrders').css("display", "none");
                $('#addSricker').css("display", "none");
                $('#allUsers').css("display", "none");
                $('#allOrders').css("display", "block");
                getOrders();
            });
            $('#features').append(content2);
        }
    }
});

async function updateUser() {
    let newUser = {
        address: $('#userAddress').val(),
        cartId: currentUser.cartId,
        country: $('#userCountry').val(),
        email: $('#userEmail').val(),
        id: currentUser.id,
        index: $('#userIndex').val(),
        name: $('#userName').val(),
        password: currentUser.password,
        role: currentUser.role,
        surname: $('#userSurname').val()
    }

    // отправляет запрос и получаем ответ
    const response = await fetch("../api/Users/Edit", {
        method: "PUT",
        headers: {
            "Content-type": "application/json",
            "Authorization": "Bearer " + sessionStorage.getItem("accessToken")
        },
        body: JSON.stringify(newUser)
    });
    // получаем данные 
    let data = await response.json();

    // если запрос прошел нормально
    if (response.ok === true) {
        console.log("All good")
    }
    else {
        // если произошла ошибка, из errorText получаем текст ошибки
        console.log("Error: ", response.status);
    }
}

function getHistory() {
    // Отправка запроса на сервер
    $.ajax({
        type: 'GET',
        dataType: 'json',
        headers: { "Authorization": "Bearer " + sessionStorage.getItem("accessToken") },
        url: "../api/Orders/UserOrders",
        // После получения ответа сервера
        success: function (orders) {
            console.log(orders);
            if (orders.length == 0) {
                $('#emptyOrders').html("<span>Your order list is empty.</span><a href=\"shop.html\">Go shopping ?</a>")
                $('#orderList').css("display", "none");
            }
            else {
                $('#emptyOrders').css("display", "none");
                $('#orderList').css("display", "block");
                let content = ``;
                $('#ordersList').html(content);
                for (let i = 0; i < orders.length; i++) {
                    let status;
                    if (orders[i].status == 0) status = "Processed";
                    if (orders[i].status == 1) status = "Completed";
                    else status = "Processed";

                    let date = orders[i].date;
                    date = date.replace("T", "  ");
                    date = date.split('.')[0];

                    // Отправка запроса на сервер
                    $.ajax({
                        type: 'GET',
                        dataType: 'json',
                        headers: {
                            "Authorization": "Bearer " + sessionStorage.getItem("accessToken"),
                            "Content-type": "application/json"
                        },
                        url: "../api/Carts/TotalPrice/" + orders[i].cartId,
                        // После получения ответа сервера
                        success: function (result) {
                            content = `
                            <tr>
                                <td class="indecor-product-name">
                                    <h4 class="title"> <a>${date}</a></h4>
                                </td>
                                <td class="indecor-product-name">
                                    <h4 class="title"> <a>${result.price}</a></h4>
                                </td>
                                <td class="indecor-product-name">
                                    <h4 class="title"> <a><b>${status}<b></a></h4>
                                </td>
                            </tr>`;
                            $('#ordersList').append(content);
                        }
                    });          
                } 
            }
        }
    });
}

function addSticker() {
    console.log("Добавление наклейки");
}

function getUsers() {
    console.log("Список всех пользователей");
}

function getOrders() {
    // Отправка запроса на сервер
    $.ajax({
        type: 'GET',
        dataType: 'json',
        headers: { "Authorization": "Bearer " + sessionStorage.getItem("accessToken") },
        url: "../api/Orders/All",
        // После получения ответа сервера
        success: function (orders) {
            $('#list').html('');
            for (let i = 0; i < orders.length; ++i) {
                if (orders.length == 0) {
                    $('#emptyAllOrders').html("<span>Your order list is empty.</span><a href=\"shop.html\">Go shopping ?</a>")
                    $('#orderAllList').css("display", "none");
                }
                else {
                    $('#emptyAllOrders').css("display", "none");
                    $('#orderAllList').css("display", "block");

                    let status;
                    if (orders[i].status == 0) status = "Processed";
                    if (orders[i].status == 1) status = "Completed";
                    else status = "Processed";

                    let date = orders[i].date;
                    date = date.replace("T", "  ");
                    date = date.split('.')[0];

                    // Отправка запроса на сервер
                    $.ajax({
                        type: 'GET',
                        dataType: 'json',
                        headers: {
                            "Authorization": "Bearer " + sessionStorage.getItem("accessToken"),
                            "Content-type": "application/json"
                        },
                        url: "../api/Carts/TotalPrice/" + orders[i].cartId,
                        // После получения ответа сервера
                        success: function (result) {
                            $('#list').append(`
                            <div style="background:#f6f6f6; border: 2px solid #efefef; border-radius: 10px;">
                            <div id="generalInformation${i}">
                                <span>Date: ${date}</span><br>
                                <span>Price: ${result.price}</span><br>
                                <span>User: ${orders[i].user.name} ${orders[i].user.surname}</span><br>
                                <span>Email: ${orders[i].user.email}</span><br>
                                <span>Address: ${orders[i].user.country}, ${orders[i].user.address}, ${orders[i].user.index}</span><br>
                                <span id="status${i}">Status: ${status}</span><a id="statusa${i}" href="javascript:changeStatus(${orders[i].id}, ${orders[i].status}, ${i})"> To change</a><br>
                            </div>
                            <div class="cart-table table-responsive" id="itemsOrder${i}">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th>Id</th>
                                            <th>Img</th>
                                            <th>Text</th>
                                            <th>Width</th>
                                            <th>Height</th>
                                            <th>Quantity</th>
                                        </tr>
                                    </thead>
                                    <tbody id="ordersAllList${i}"></tbody>
                                </table>
                            </div>
                            </div><br>`);

                            let content = ``;
                            $(`#ordersAllList${i}`).html(content);
                            for (let j = 0; j < orders[i].cart.items.length; j++) {
                                content = `<tr>
                                        <td class="indecor-product-name">
                                            <h4 class="title"> <a>${orders[i].cart.items[j].sticker.id}</a></h4>
                                        </td>
                                        <td class="indecor-product-thumbnail" >
                                            <a><img src="assets/img/shop/cart/table1.jpg" alt="Image-HasTech"></a>
                                        </td>
                                        <td class="indecor-product-name">
                                            <h4 class="title"> <a>${orders[i].cart.items[j].sticker.text}</a></h4>
                                        </td>
                                        <td class="indecor-product-name">
                                            <h4 class="title"> <a>${orders[i].cart.items[j].sticker.width}</a></h4>
                                        </td>
                                        <td class="indecor-product-name">
                                            <h4 class="title"> <a>${orders[i].cart.items[j].sticker.height}</a></h4>
                                        </td>
                                        <td class="indecor-product-name">
                                            <h4 class="title"> <a>${orders[i].cart.items[j].quantity}</a></h4>
                                        </td>
                                    </tr>`;
                                $(`#ordersAllList${i}`).append(content);
                        
                            }
                        }
                    });
                }
            }
        }
    });
}

function changeStatus(id, status, i) {
    status = status == 0 ? 1 : 0;
    // Отправка запроса на сервер
    $.ajax({
        type: 'PUT',
        dataType: 'json',
        headers: {
            "Authorization": "Bearer " + sessionStorage.getItem("accessToken"),
            "Content-type": "application/json"
        },
        url: "../api/Orders/ChangeStatus/" + id + "/" + status,
        // После получения ответа сервера
        success: function (result) {
            let statusText;
            if (status == 1) statusText = "Completed";
            else statusText = "Processed";

            $(`#status${i}`).text(`Status: ${statusText}`);
            $(`#statusa${i}`).attr("href", `javascript:changeStatus(${id}, ${status}, ${i})`);
        }
    });
}