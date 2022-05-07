/*init();

function init() {
    // Отправка запроса на сервер
    $.ajax({
        type: 'GET',
        dataType: 'json',
        url: "../api/Stickers/AllQuantityInCategory/0",
        // После получения ответа сервера
        success: function (result) {
            let currentPage = 1;
            const quantityPerPage = 1;
            

            let quantityLabels = result["quantity"];
            let right = (currentPage * quantityPerPage >= quantityLabels) ? quantityLabels : (quantityPerPage * currentPage);
            $('#showing').html("Showing " + (1 + (currentPage - 1) * quantityPerPage) + " - " + right + " of " + quantityLabels + " result");
            let maxPage = Math.ceil(quantityLabels / quantityPerPage);

            $('#currentPage').html(currentPage + " of " + maxPage);
            if (currentPage >= maxPage) {
                $('#nextPage').addClass('disabled');
                $('#nextPage').addClass('active');
            }

            getPage(0, currentPage, quantityPerPage);

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
                    getPage(0, currentPage, quantityPerPage);
                }
            });

            // Настройка работы кнопки перехода на следующую страницу
            $('#nextPage').on('click', function () {
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
                    getPage(0, currentPage, quantityPerPage);
                }
            });
            
            // Отправка запроса на сервер
            $.ajax({
                type: 'GET',
                dataType: 'json',
                url: "../api/Categories/All",
                // После получения ответа сервера
                success: function (categories) {
                    getPage(0, currentPage, quantityPerPage);
                    let currentCategory = 0;
                    let content = $('<li id="0category" class="active"><a>All<span class="fa fa-angle-double-right"></span></a></li>');
                    content.on('click', function () {
                        console.log("click: " + 0);
                        $(`#${(currentCategory)}category`).removeClass('active');
                        currentCategory = 0;
                        $(`#${(currentCategory)}category`).addClass('active');
                        getPage(0, currentPage, quantityPerPage);
                    });
                    $('#categories').append(content);

                    for (let i = 0; i < categories.length; i++) {
                        let idsha = `${(i + 1)}category`;
                        content[i] = $(`<li id="${idsha}" class=""><a>${categories[i]["name"]}<span class="fa fa-angle-double-right"></span></a></li>`);
                        content[i].on('click', function () {
                            console.log("click: " + (i + 1));
                            $(`#${(currentCategory)}category`).removeClass('active');
                            currentCategory = i + 1;
                            $(`#${(currentCategory)}category`).addClass('active');
                            getPage(categories[i]["id"], currentPage, quantityPerPage);
                        });
                        $('#categories').append(content[i]);
                    }
                }
            });
        }
    });
}

function getPage(idCategory, currentPage, quantityPerPage) {
    // Отправка запроса на сервер
    $.ajax({
        type: 'GET',
        dataType: 'json',
        url: "../api/Stickers/PageInCategory/" + idCategory + "/" + currentPage + "/" + quantityPerPage,
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
                content += "</a> ";
                content += "<div class=\"product-action\">";
                content += "<div class=\"addto-wrap\">";
                content += "<a class=\"add-cart\" href=\"cart.html\">";
                content += "<i class=\"zmdi zmdi-shopping-cart-plus icon\"></i>";
                content += "</a> ";
                content += "</div> ";
                content += "</div> ";
                content += "</div> ";
                content += "<div class=\"product-desc\">";
                content += "<div class=\"product-info\">";
                content += "<h4 class=\"title\"><a href=\"single-product-simple.html\">" + labels[i]["firm"] + "</a></h4>";
                content += "<div class=\"prices\" >";
                content += "<span class=\"price\">Price: " + labels[i]["price"] + " sticker</span>";
                content += "</div>";
                content += "</div>";
                content += "</div>";
                content += "</div>";
                content += "</div > ";
                content += "<!--== End Shop Item == --> ";
                content += "</div>";
            }
            $('#stickers').html(content);
        }
    });
}
*/




 init();

function init() {
    // Отправка запроса на сервер
    $.ajax({
        type: 'GET',
        dataType: 'json',
        url: "../api/Categories/All",
        // После получения ответа сервера
        success: function (categories) {
            //getPage(0, currentPage, quantityPerPage);
            first(0)
            let currentCategory = 0;
            let content = $('<li id="0category" class="active"><a>All<span class="fa fa-angle-double-right"></span></a></li>');
            content.on('click', function () {
                console.log("click: " + 0);
                $(`#${(currentCategory)}category`).removeClass('active');
                currentCategory = 0;
                $(`#${(currentCategory)}category`).addClass('active');
                //getPage(0, currentPage, quantityPerPage);
                first(0)
            });
            $('#categories').append(content);

            for (let i = 0; i < categories.length; i++) {
                let idsha = `${(i + 1)}category`;
                content[i] = $(`<li id="${idsha}" class=""><a>${categories[i]["name"]}<span class="fa fa-angle-double-right"></span></a></li>`);
                content[i].on('click', function () {
                    console.log("click: " + (i + 1));
                    $(`#${(currentCategory)}category`).removeClass('active');
                    currentCategory = i + 1;
                    $(`#${(currentCategory)}category`).addClass('active');
                    //getPage(categories[i]["id"], currentPage, quantityPerPage);
                    first(categories[i]["id"])
                });
                $('#categories').append(content[i]);
            }
        }
    });

}
function first(idCategory) {
    // Отправка запроса на сервер
    $.ajax({
        type: 'GET',
        dataType: 'json',
        url: "../api/Stickers/AllQuantityInCategory/" + idCategory,
        // После получения ответа сервера
        success: function (result) {
            let currentPage = 1;
            const quantityPerPage = 1;

            let quantityLabels = result["quantity"];
            let right = (currentPage * quantityPerPage >= quantityLabels) ? quantityLabels : (quantityPerPage * currentPage);
            $('#showing').html("Showing " + (1 + (currentPage - 1) * quantityPerPage) + " - " + right + " of " + quantityLabels + " result");
            let maxPage = Math.ceil(quantityLabels / quantityPerPage);
            console.log("1. From first: maxPage = " + maxPage + ", quantityLabels = " + quantityLabels + ", quantityPerPage = " + quantityPerPage);
            $('#currentPage').html(currentPage + " of " + maxPage);
            if (currentPage >= maxPage) {
                $('#nextPage').addClass('disabled');
                $('#nextPage').addClass('active');
            }
            if (currentPage <= 0) {
                $('#prevPage').addClass('disabled');
                $('#prevPage').addClass('active');
            }
            console.log("2. maxPage = " + maxPage + ", quantityLabels = " + quantityLabels + ", quantityPerPage = " + quantityPerPage + ", idCategory = " + idCategory);
            getPage(idCategory, currentPage, quantityPerPage);
            console.log("3. maxPage = " + maxPage + ", quantityLabels = " + quantityLabels + ", quantityPerPage = " + quantityPerPage + ", idCategory = " + idCategory);
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
                    getPage(idCategory, currentPage, quantityPerPage);
                }
            });
            console.log("4. maxPage = " + maxPage + ", quantityLabels = " + quantityLabels + ", quantityPerPage = " + quantityPerPage + ", idCategory = " + idCategory);
            // Настройка работы кнопки перехода на следующую страницу
            $('#nextPage').on('click', function () {
                console.log("5. maxPage = " + maxPage + ", quantityLabels = " + quantityLabels + ", quantityPerPage = " + quantityPerPage + ", idCategory = " + idCategory);
                console.log("6. nextPage.click with currentPage + 1 = " + (currentPage + 1) + ", maxPage = " + maxPage);
                console.log("7. nextPage.click maxPage = " + maxPage + ", quantityLabels = " + quantityLabels + ", quantityPerPage = " + quantityPerPage + ", idCategory = " + idCategory);

                if (currentPage + 1 <= maxPage) {
                    console.log("Условие <= выполнилось")
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
                    getPage(idCategory, currentPage, quantityPerPage);
                }
            });
        }
    });
}

function getPage(idCategory, currentPage, quantityPerPage) {
    console.log("idCategory: " + idCategory + " ,currentPage: " + currentPage + " ,quantityPerPage: " + quantityPerPage);
    // Отправка запроса на сервер
    $.ajax({
        type: 'GET',
        dataType: 'json',
        url: "../api/Stickers/PageInCategory/" + idCategory + "/" + currentPage + "/" + quantityPerPage,
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
                content += "</a> ";
                content += "<div class=\"product-action\">";
                content += "<div class=\"addto-wrap\">";
                content += "<a class=\"add-cart\" href=\"cart.html\">";
                content += "<i class=\"zmdi zmdi-shopping-cart-plus icon\"></i>";
                content += "</a> ";
                content += "</div> ";
                content += "</div> ";
                content += "</div> ";
                content += "<div class=\"product-desc\">";
                content += "<div class=\"product-info\">";
                content += "<h4 class=\"title\"><a href=\"single-product-simple.html\">" + labels[i]["firm"] + "</a></h4>";
                content += "<div class=\"prices\" >";
                content += "<span class=\"price\">Price: " + labels[i]["price"] + " sticker</span>";
                content += "</div>";
                content += "</div>";
                content += "</div>";
                content += "</div>";
                content += "</div > ";
                content += "<!--== End Shop Item == --> ";
                content += "</div>";
            }
            $('#stickers').html(content);
        }
    });
}