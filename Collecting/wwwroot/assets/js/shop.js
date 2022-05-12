first(0, "При прогрузке", "category");
init();

function init() {
    // Отправка запроса на сервер
    $.ajax({
        type: 'GET',
        dataType: 'json',
        url: "../api/Categories/All",
        // После получения ответа сервера
        success: function (categories) {
            let currentCategory = 0;
            let content = $('<li id="0category" class="active"><a>All<span class="fa fa-angle-double-right"></span></a></li>');
            content.on('click', function () {
                $(`#${(currentCategory)}category`).removeClass('active');
                currentCategory = 0;
                $(`#${(currentCategory)}category`).addClass('active');
                first(0, "При клике на All", "category")
            });
            $('#categories').append(content);

            for (let i = 0; i < categories.length; i++) {
                let idsha = `${(i + 1)}category`;
                content[i] = $(`<li id="${idsha}" class=""><a>${categories[i]["name"]}<span class="fa fa-angle-double-right"></span></a></li>`);
                content[i].on('click', function () {
                    $(`#${(currentCategory)}category`).removeClass('active');
                    currentCategory = i + 1;
                    $(`#${(currentCategory)}category`).addClass('active');
                    first(categories[i]["id"], `При клике на ${categories[i]["name"]}`, "category")
                });
                $('#categories').append(content[i]);
            }
        }
    });
    
    // Настройка работы кнопки поиска
    $('#searchButton').on('click', function () {
        first(0, "При поиске", "search");
    });
}
function first(idCategory, flag, type) {
    let newUrl;
    if (type == "category")
        newUrl = "../api/Stickers/AllQuantityInCategory/" + idCategory;
    else
        newUrl = "../api/Stickers/SearchQuantity/" + $('#search2').val();

    // Отправка запроса на сервер
    $.ajax({
        type: 'GET',
        dataType: 'json',
        url: newUrl,
        // После получения ответа сервера
        success: function (result) {
            let currentPage = 1;
            const quantityPerPage = 2;
            if (flag != "При прогрузке") {
                $('#nextPage').unbind("click");
                $('#prevPage').unbind("click");
            }
            let quantityLabels = result["quantity"];
            let right = (currentPage * quantityPerPage >= quantityLabels) ? quantityLabels : (quantityPerPage * currentPage);
            $('#showing').html("Showing " + (1 + (currentPage - 1) * quantityPerPage) + " - " + right + " of " + quantityLabels + " result");
            let maxPage = Math.ceil(quantityLabels / quantityPerPage);
            
            $('#currentPage').html(currentPage + " of " + maxPage);

            $('#nextPage').removeClass('disabled');
            $('#nextPage').removeClass('active');
            $('#prevPage').removeClass('disabled');
            $('#prevPage').removeClass('active');

            if (currentPage == maxPage) {
                $('#nextPage').addClass('disabled');
                $('#nextPage').addClass('active');
            }
            if (currentPage - 1 <= 0) {
                $('#prevPage').addClass('disabled');
                $('#prevPage').addClass('active');
            }

            getPage(idCategory, currentPage, quantityPerPage, type);
            
            // Настройка работы кнопки перехода на предыдущую страницу
            $('#prevPage').on('click', function () {
                if (currentPage - 1 > 0) {
                    $('#nextPage').removeClass('disabled');
                    $('#nextPage').removeClass('active');
                    if (currentPage - 2 <= 0) {
                        $(this).addClass('disabled');
                        $(this).addClass('active');
                    }
                    currentPage--;
                    right = (currentPage * quantityPerPage >= quantityLabels) ? quantityLabels : (quantityPerPage * currentPage);
                    $('#showing').html("Showing " + (1 + (currentPage - 1) * quantityPerPage) + " - " + right + " of " + quantityLabels + " result");
                    $('#currentPage').html(currentPage + " of " + maxPage);
                    getPage(idCategory, currentPage, quantityPerPage, type);
                }
            });

            // Настройка работы кнопки перехода на следующую страницу
            $('#nextPage').on('click', function (e) {
                if (currentPage + 1 <= maxPage) {
                    $('#prevPage').removeClass('disabled');
                    $('#prevPage').removeClass('active');
                    if (currentPage + 2 > maxPage) {
                        $(this).addClass('disabled');
                        $(this).addClass('active');
                    }
                    currentPage++;
                    $('#currentPage').html(currentPage + " of " + maxPage);
                    right = (currentPage * quantityPerPage >= quantityLabels) ? quantityLabels : (quantityPerPage * currentPage);
                    $('#showing').html("Showing " + (1 + (currentPage - 1) * quantityPerPage) + " - " + right + " of " + quantityLabels + " result");
                    getPage(idCategory, currentPage, quantityPerPage, type);
                }
            });
        }
    });
}

function getPage(idCategory, currentPage, quantityPerPage, type) {
    let newUrl;
    if (type == "category")
        newUrl = "../api/Stickers/PageInCategory/" + idCategory + "/" + currentPage + "/" + quantityPerPage;
    else
        newUrl = "../api/Stickers/Search/" + $('#search2').val();

    // Отправка запроса на сервер
    $.ajax({
        type: 'GET',
        dataType: 'json',
        url: newUrl,
        // После получения ответа сервера
        success: function (labels) {
            console.log(labels);
            var content = '';
            for (let i = 0; i < labels.length; i++) {
                content += "<div class=\"col-sm-6 col-xl-4\" >";
                content += "<!--== Start Shop Item == -->";
                content += "<div class=\"product-item\">";
                content += "<div class=\"inner-content\">";
                content += "<div class=\"product-thumb\">";
                content += "<a href=\"single-product-simple.html\">";
                content += "<img class=\"w-100\" src=\"assets/img/shop/2.jpg\" alt=\"Image-HasTech\">";
                content += "</a>";
                content += "<div class=\"product-action\">";
                content += "<div class=\"addto-wrap\">";
                content += "<a class=\"add-cart\" href=\"javascript:AddToCart(" + labels[i]["id"] + ", 1)\">";
                content += "<i class=\"zmdi zmdi-shopping-cart-plus icon\"></i>";
                content += "</a>";
                content += "<a class=\"add-quick-view id=\"viewItem\"\" href=\"javascript:wow(" + labels[i]["id"] + ");\">"
                content += "<i class=\"zmdi zmdi-search icon\"></i>"
                content += "</a>"
                content += "</div>";
                content += "</div>";
                content += "</div>";
                content += "<div class=\"product-desc\">";
                content += "<div class=\"product-info\">";
                content += "<h4 class=\"title\"><a href=\"single-product.html\">" + labels[i]["firm"] + "</a></h4>";
                content += "<div class=\"prices\">";
                content += "<span class=\"price\">Price: " + labels[i]["price"] + " sticker</span>";
                content += "</div>";
                content += "</div>";
                content += "</div>";
                content += "</div>";
                content += "</div>";
                content += "<!--== End Shop Item == -->";
                content += "</div>";
            }
            $('#stickers').html(content);
        }
    });
}

function AddToCart(id, quantity) {
    const token = sessionStorage.getItem("accessToken");
    if (token != null) {
        if (quantity == -1) {
            quantity = Number($('#quantity' + id).attr("value"));
        }
        // Отправка запроса на сервер
        $.ajax({
            type: 'POST',
            dataType: 'json',
            headers: {
                "Authorization": "Bearer " + token,
                "Content-type": "application/json"
            },
            url: "../api/CartItems/Create/" + id + "/" + quantity,
            // После получения ответа сервера
            success: function (result) {
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
        });
    }
    else {
        alert("Для добавления товара в корзину необходимо авторизоваться")
    }
}

function wow(id) {
    // Отправка запроса на сервер
    $.ajax({
        type: 'GET',
        dataType: 'json',
        headers: {
            "Content-type": "application/json"
        },
        url: "../api/Stickers/Sticker/" + id,
        // После получения ответа сервера
        success: function (sticker) {
            $.ajax({
                type: 'GET',
                dataType: 'json',
                headers: {
                    "Content-type": "application/json"
                },
                url: "../api/Categories/Category/" + sticker.categoryID,
                // После получения ответа сервера
                success: function (category) {
                    let content = `
                        <div class="product-quick-view-inner">
                            <div class="product-quick-view-content">
                                <button type="button" class="btn-close" id="close">
                                    <span class="close-icon"><i class="fa fa-close"></i></span>
                                </button>
                                <div class="row">
                                    <div class="col-lg-6 col-md-6 col-12">
                                        <div class="thumb">
                                            <img src="assets/img/shop/quick-view1.jpg" alt="Alan-Shop">
                                            </div>
                                        </div>
                                        <div class="col-lg-6 col-md-6 col-12">
                                            <div class="content">
                                                <h4 class="title">${sticker.firm}</h4>
                                                <div class="prices">
                                                    <span class="price">${sticker.price} sticker</span>
                                                </div>
                                                <table>
                                                <tr><th>Year</th><th>Country</th><th>Material</th></tr>
                                                <tr><td>${sticker.year}</td><td>${sticker.country}</td><td>${sticker.material}</td></tr>
                                                <tr><th>Width</th><th>Height</th><th>Max quantity</th></tr>
                                                <tr><td>${sticker.width}</td><td>${sticker.height}</td><td>${sticker.quantity}</td></tr>
                                                <tr><th>Form</th><th>Text</th><th>Category</th></tr>
                                                <tr><td>${sticker.form}</td><td>${sticker.text}</td><td>${category.name}</td></tr>
                                                </table>
                                                <div class="action-top">
                                                    <div class="pro-qty">
                                                        <input type="text" id="quantity${sticker.id}" title="Quantity" value="1" />
                                                        <div class="inc qty-btn" onclick="countPlus(${sticker.id}, ${sticker.quantity})">+</div>
                                                        <div class="dec qty-btn" onclick="countMinus(${sticker.id})">-</div>
                                                    </div>
                                                    <button class="btn btn-black" onclick="AddToCart(${sticker.id}, -1)">Add to cart</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="canvas-overlay"></div>
                        `;
                    $('#popUp').html(content);
                }
            });
        }
    });

    $('#overlay').fadeIn(250, function () {
        $('#popUp')
            .css('display', 'block')
            .animate({ opacity: 1, top: '55%' }, 490);
    });

    /*по нажатию на крестик закрывать окно*/
    $('#close, #overlay').click(function () {
        $('#popUp')
            .animate({ opacity: 0, top: '35%' }, 490,
            function () {
                $(this).css('display', 'none');
                $('#overlay').fadeOut(220);
            }
        );
    });
}

// Настройка работы кнопки увеличения количества товаров
function countPlus(id, quantity) {
    let old = Number($(`#quantity${id}`).attr("value"));
    if ((old + 1) <= quantity) {
        $(`#quantity${id}`).attr("value", (old + 1));
    }
}

// Настройка работы кнопки уменьшения количества товаров
function countMinus(id) {
    let old = Number($(`#quantity${id}`).attr("value"));
    if ((old - 1) > 0)
    $(`#quantity${id}`).attr("value", (old - 1));
}