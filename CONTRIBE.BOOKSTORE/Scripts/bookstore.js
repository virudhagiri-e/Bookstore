// Global variables
var userId = "";
var bookCart = null;

// function to make AJAX call
function makeAjaxCall(url, params, successCallback) {
    var jsonData = JSON.stringify(params);

    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8",
        data: jsonData,
        dataType: "json",
        success: successCallback,
        error: showTechnicalError
    });
}

$(document).ajaxStart(function () {
    $("#wait").css("display", "block");
});

$(document).ajaxComplete(function () {
    $("#wait").css("display", "none");
});

// Login
$("#btnLogin").click(function () {
    userId = $("#txtLogin").val().trim();
    if (userId) {
        $("#userId").text(userId.toUpperCase());
        $("#loggedIn").show();
        $("#notLoggedIn").hide();
        $("#bookCartDivId").show();
        $("#orderDetailsDivId").show();
    }
    return false;
});

// Logout
$("#btnLogout").click(function () {
    userId = "";
    $("#loggedIn").hide();
    $("#notLoggedIn").show();
    $("#bookCartDivId").hide();
    $("#orderDetailsDivId").hide();
    $("#tblBookCart tbody > tr").remove();
    return false;
});

// Book search
$("#btnSearch").click(function () {
    var title = $("#txtTitleSearch").val();
    var author = $("#txtAuthorSearch").val();
    var selectedValue = $('input[name=rdbtnAndOr]:checked').val();
    var params = { searchString: 'title eq ' + title + ' ' + selectedValue + ' ' + 'author eq ' + author };

    makeAjaxCall("/Home/SearchBookAsync", params, searchSuccessCallback);

    return false;
});

function searchSuccessCallback(message) {
    $("#tblSearchResult tbody > tr").remove();

    if (!message || message.length === 0) {
        return showError('No Matching Results!');
    }

    $.each(message, function (i, book) {
        $("#tblSearchResult tbody").append(
            '<tr><td align="center">' + (i + 1) + '</td>' +
            '<td>' + book.Title + '</td>' +
            '<td>' + book.Author + '</td>' +
            '<td>' + book.Price.formatMoney(2, "SEK", ".", ",") + '</td>' +
            '<td><input class="addBook" bookId="' + book.ID + '" type="button" value="Add to Cart" /></td></tr>');
    });

    $("#tblSearchResult").find(".addBook").click(function () {
        addToCart($(this).attr("bookId"));
    });
}

// Add book to cart
function addToCart(bookId) {
    if (!userId) {
        alert("You must login!");
        return;
    }

    var params = { userID: userId, bookID: bookId };
    makeAjaxCall("/Home/AddBookAsync", params, addOrRemoveBookSuccessCallback);
}

// Remove book from cart
function removeFromCart(bookId) {
    if (!userId) {
        alert("You must login!");
        return;
    }

    var params = { userID: userId, bookID: bookId };
    makeAjaxCall("/Home/RemoveBookAsync", params, addOrRemoveBookSuccessCallback);
}

// Update quantity book from cart
function updateQuantity(bookId, quantity) {
    if (!userId) {
        alert("You must login!");
        return;
    }

    var params = { userID: userId, bookID: bookId, quantity: quantity };
    makeAjaxCall("/Home/UpdateQuantityAsync", params, addOrRemoveBookSuccessCallback);
}

function addOrRemoveBookSuccessCallback(message) {
    bookCart = message;

    $("#tblBookCart tbody > tr").remove();

    if (!message || !message.BookCartItems || message.BookCartItems.length === 0) {
        $("#btnPlaceOrder").hide();
        return showError('Cart Empty!');
    }

    $("#btnPlaceOrder").show();

    $.each(message.BookCartItems, function (i, bookCartItem) {
        $("#tblBookCart tbody").append(
            '<tr><td align="center">' + (i + 1) + '</td>' +
            '<td>' + bookCartItem.Book.Title + '</td>' +
            '<td>' + bookCartItem.Book.Author + '</td>' +
            '<td>' + bookCartItem.Book.Price.formatMoney(2, "SEK", ".", ",") + '</td>' +
            '<td><input style="align-content:center" class="updateQuantity" bookId="' + bookCartItem.Book.ID + '" type="text" value="' + bookCartItem.Quantity + '" maxlength="2" size="3" /></td>' +
            '<td><input class="removeBook" bookId="' + bookCartItem.Book.ID + '" type="button" value="Remove" /></td></tr>');
    });

    $("#tblBookCart tbody").append(
        '<tr><td colspan="3" align="right">Total</td>' +
        '<td align="center">' + message.TotalPrice.formatMoney(2, "SEK", ".", ",") + '</td>' +
        '<td align="center">' + message.TotalQuantity + '</td>' +
        '<td></td></tr>');

    $("#tblBookCart").find(".removeBook").click(function () {
        removeFromCart($(this).attr("bookId"));
    });

    $("#tblBookCart").find(".updateQuantity").change(function () {
        var bookId = $(this).attr("bookId");
        var quantity = $(this).val();
        updateQuantity(bookId, quantity);
    });

    if (message.BookCartItems.length === 0) {
        $("#btnPlaceOrder").hide();
    }
}

// Place order
$("#btnPlaceOrder").click(function () {
    if (!userId) {
        alert("You must login!");
        return;
    }

    var params = { userID: userId, bookCart: bookCart };
    makeAjaxCall("/Home/CreateOrderAsync", params, placeOrderSuccessCallback);
});

function placeOrderSuccessCallback(message) {
    $("#tblOrderDetails tbody > tr").remove();

    if (!message || !message.PurchasedBooks || (message.PurchasedBooks.length === 0 && !message.NotInStockBooks && message.NotInStockBooks.length === 0)) {
        return showError('Order not placed!');
    }

    $("#btnPlaceOrder").show();

    if (message.PurchasedBooks.length > 0) {
        $.each(message.PurchasedBooks, function (i, bookCart) {
            $("#tblOrderDetails tbody").append(
                '<tr><td align="center">' + (i + 1) + '</td>' +
                '<td>' + bookCart.Book.Title + '</td>' +
                '<td>' + bookCart.Book.Author + '</td>' +
                '<td>' + bookCart.Book.Price.formatMoney(2, "SEK", ".", ",") + '</td>' +
                '<td align="center">' + bookCart.Quantity + '</td></tr>');
        });

        $("#tblOrderDetails tbody").append(
            '<tr><td colspan="3" align="right">Total</td>' +
            '<td align="center">' + message.TotalPrice.formatMoney(2, "SEK", ".", ",") + '</td>' +
            '<td align="center">' + message.TotalQuantity + '</td>' +
            '<td></td></tr>');
    }

    if (message.NotInStockBooks.length > 0) {
        $("#tblOrderDetails tbody").append(
            '<tr> <td colspan="5">Not in stock</td></tr >');

        $.each(message.NotInStockBooks, function (i, bookCart) {
            $("#tblOrderDetails tbody").append(
                '<tr><td align="center">' + (i + 1) + '</td>' +
                '<td>' + bookCart.Book.Title + '</td>' +
                '<td>' + bookCart.Book.Author + '</td>' +
                '<td>' + bookCart.Book.Price.formatMoney(2, "SEK", ".", ",") + '</td>' +
                '<td align="center">' + bookCart.Quantity + '</td></tr>');
        });
    }

    $("#tblBookCart tbody > tr").remove();
    $("#btnPlaceOrder").hide();
    bookCart = null;
}

function showError(message) {
    alert(message);
}

function showTechnicalError(jqXHR, status, errorThrown) {
    alert('Technical Error Occurred! : ' + errorThrown);
}

Number.prototype.formatMoney = function (places, symbol, thousand, decimal) {
    places = !isNaN(places = Math.abs(places)) ? places : 2;
    symbol = symbol !== undefined ? symbol : "SEK";
    thousand = thousand || ",";
    decimal = decimal || ".";
    var number = this,
        negative = number < 0 ? "-" : "",
        i = parseInt(number = Math.abs(+number || 0).toFixed(places), 10) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    return negative + (j ? i.substr(0, j) + thousand : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousand) + (places ? decimal + Math.abs(number - i).toFixed(places).slice(2) : "") + " " + symbol;
};
