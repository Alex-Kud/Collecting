getCart();

function getCart() {
    // Отправка запроса на сервер
    $.ajax({
        type: 'GET',
        dataType: 'json',
        headers: { "Authorization": "Bearer " + sessionStorage.getItem("accessToken") },
        url: "../api/Carts/CartUser",
        // После получения ответа сервера
        success: function (labels) {
            var content = '';
            $('#productsInCart').html(content);
            if (labels.items.length == 0) {
                $('#emptyCart').html("<span>Your shopping cart is still empty.</span><a href=\"shop.html\">Go shopping ?</a>")
                $('#cartList').css("display", "none");
            }
            else {
                $('#emptyCart').css("display", "none");
                $('#cartList').css("display", "block");
                for (let i = 0; i < labels.items.length; i++) {
                    console.log(labels);
                    console.log(labels["items"][i]["sticker"]["img"]);
                    content += `<tr>`
                    content += `    <td class="indecor-product-remove">`
                    content += `        <a href="javascript:removeItem(${labels["items"][i]["id"]})"> <i class="fa fa-times"></i></a>`
                    content += `    </td>`
                    content += `    <td class="indecor-product-thumbnail" >`
                    content += `        <a><img width="60" height="60" src="${labels["items"][i]["sticker"]["img"]}" alt="Image-HasTech"></a>`
                    content += `    </td>`
                    content += `    <td class="indecor-product-name">`
                    content += `        <h4 class="title"> <a href="single-product.html">${labels["items"][i]["sticker"]["firm"]}</a></h4>`
                    content += `    </td>`
                    content += `    <td class="indecor-product-price"> <span class="price">${labels["items"][i]["unitPrice"]} sticker</span></td>`
                    content += `    <td class="indecor-product-quantity">`
                    content += `        <div class="pro-qty">`
                    content += `            <input type="text" id="quantity${labels["items"][i]["id"]}" readonly min="0" title="Quantity" value="${labels["items"][i]["quantity"]}">`
                    content += `            <div class="inc qty-btn" onclick="countPlus(${labels["items"][i]["id"]})">+</div>`
                    content += `            <div class="dec qty-btn" onclick="countMinus(${labels["items"][i]["id"]})">-</div>`
                    content += `        </div>`
                    content += `    </td>`
                    content += `    <td class="product-subtotal"> <span class="price">${labels["items"][i]["totalPrice"]} sticker</span></td>`
                    content += `</tr>`
                }
                $('#productsInCart').append(content);
                getTotal();
            }
        }
    });
}

// Настройка работы кнопки увеличения количества товаров
function countPlus(id) {
    let old = Number($(`#quantity${id}`).attr("value"));
    $(`#quantity${id}`).attr("value", (old + 1));
    updateCart(id);
}

// Настройка работы кнопки уменьшения количества товаров
function countMinus(id) {
    let old = Number($(`#quantity${id}`).attr("value"));
    $(`#quantity${id}`).attr("value", (old - 1));
    updateCart(id);
}

// Обновление количества товаров в корзине
function updateCart(id) {
    let newQuantity = Number($(`#quantity${id}`).attr("value"));
    // Отправка запроса на сервер
    $.ajax({
        type: 'POST',
        dataType: 'json',
        headers: {
            "Authorization": "Bearer " + sessionStorage.getItem("accessToken"),
            "Content-type": "application/json"
        },
        url: "../api/CartItems/ChangeQuantity/" + id + "/" + newQuantity,
        // После получения ответа сервера
        success: function (labels) {
            getCart();
        }
    });
}

// Получение общей стоимости заказа
function getTotal() {
    // Отправка запроса на сервер
    $.ajax({
        type: 'GET',
        dataType: 'json',
        headers: {
            "Authorization": "Bearer " + sessionStorage.getItem("accessToken"),
            "Content-type": "application/json"
        },
        url: "../api/Carts/TotalPriceUser",
        // После получения ответа сервера
        success: function (result) {
            $('#cartTotal').html(result["price"] + " stickers");
        }
    });
}

// Очистка корзины
function clearCart() {
    // Отправка запроса на сервер
    $.ajax({
        type: 'DELETE',
        dataType: 'json',
        headers: {
            "Authorization": "Bearer " + sessionStorage.getItem("accessToken"),
            "Content-type": "application/json"
        },
        url: "../api/Carts/Delete",
        // После получения ответа сервера
        success: function (result) {
            getCart();
        }
    });
}

// Удаление одного товара из корзины
function removeItem(id) {
    // Отправка запроса на сервер
    $.ajax({
        type: 'DELETE',
        dataType: 'json',
        headers: {
            "Authorization": "Bearer " + sessionStorage.getItem("accessToken"),
            "Content-type": "application/json"
        },
        url: "../api/CartItems/Delete/" + id,
        // После получения ответа сервера
        success: function (result) {
            getCart();
        }
    });
}

function makingAnOrder() {
    // Отправка запроса на сервер
    $.ajax({
        type: 'POST',
        dataType: 'json',
        headers: {
            "Authorization": "Bearer " + sessionStorage.getItem("accessToken"),
        },
        url: "../api/Orders/Create",
        // После получения ответа сервера
        success: function (order) {
            window.location.href = 'checkout.html';
        }
    });
}