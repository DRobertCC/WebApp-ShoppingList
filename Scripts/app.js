var currentList = {};

function createShoppingList() {
    currentList.name = $("#shoppingListName").val();
    currentList.items = new Array(); // Arraylist for the items

    if (currentList.name != "") {
        // Web service call
        $.ajax({
            type: "POST", // mit csináljon
            dataType: "json", // = send the response as Json (JS object) so we can access its properties.
            url: "api/ShoppingListsEF/", // the controller's address
            data: currentList,
            success: function (result) { // callback siker esetén
                currentList = result;
                showShoppingList();
                history.pushState({ id: result.id }, result.name, "?id=" + result.id);
            },
            error: function () {
                console.error("Error!")
            }
        });
    }
}

function showShoppingList() {
    $("#shoppingListTitle").html(currentList.name);
    $("#shoppingListItems").empty();

    $("#createListDiv").hide();
    $("#shoppingListDiv").show();

    $("#newItemName").val("");
    // Adding keyboard control:
    $("#newItemName").focus(); // Cursor jumps here
    $("#newItemName").unbind("keyup"); // To not create a list every time the user (re)visits the first page and hits return.
    $("#newItemName").keyup(function (event) {
        if (event.keyCode == 13) {
            addItem();
        }
    }); // if there was a return keypress then add the item

}

function addItem() {
    var newItem = {}; // creating an empty object to store an item
    newItem.name = $("#newItemName").val();
    newItem.shoppingListId = currentList.id; // a current listához kötjük az újat

    // console.info(jQuery.inArray(newItem, currentList.items));
    if (newItem.name != "" && (jQuery.inArray(newItem.name, currentList.items) == -1)) // It is a jQuery function that returns the index position of the value when it finds the value, otherwise -1.
    {
        $.ajax({
            type: "POST", // mit csináljon
            dataType: "json", // = send the response as Json (JS object) so we can access its properties.
            url: "api/ItemsEF/", // the controller's address
            data: newItem, // this we send to the backend service
            success: function (result) { // callback siker esetén
                currentList = result; // frissítem a frontendet a backendtől jövő eredménnyel
                drawItems();
                $("#newItemName").val(''); // clearing the textbox after adding the new item.
            },
        });
    }
}

function drawItems() {
    var $list = $("#shoppingListItems").empty();

    for (var i = 0; i < currentList.items.length; i++) {
        var currentItem = currentList.items[i];
        var $li = $("<li>").html(currentItem.name)
            .attr("id", "item_" + i);
        var $deleteBtn =
            $("<button onclick='deleteItem(" + currentItem.id + ")'>D</button>").appendTo($li);
        var $checkBtn =
            $("<button onclick='checkItem(" + currentItem.id + ")'>C</button>").appendTo($li);

        if (currentItem.checked) {
            $li.addClass("checked");
        }

        $li.appendTo($list);
    }
}

function deleteItem(itemId) {
    $.ajax({
        type: "DELETE", // mit csináljon
        dataType: "json", // = send the response as Json (JS object) so we can access its properties.
        url: "api/ItemsEF/" + itemId, // the controller's address
        success: function (result) { // callback siker esetén
            currentList = result; // frissítem a frontendet a backendtől jövő eredménnyel
            drawItems();
        },
    });
}

function checkItem(itemId) {
    /* Dummy
    if ($("#item_" + index).hasClass("checked")) {
        $("#item_" + index).removeClass("checked");
    }
    else {
        $("#item_" + index).addClass("checked");
    }*/
    var changedItem = {};

    for (var i = 0; i < currentList.items.length; i++) { // First find the item with the given itemId ...
        if (currentList.items[i].id == itemId) {
            changedItem = currentList.items[i];
        }
    }

    changedItem.checked = !changedItem.checked; // ... then flip the checked property ...
    // ... then send the whole thing to the service
    $.ajax({
        type: "PUT", // mit csináljon
        dataType: "json", // = send the response as Json (JS object) so we can access its properties.
        url: "api/ItemsEF/" + itemId, // the controller's address
        data: changedItem,
        success: function (result) { // callback siker esetén
            changedItem = result; // frissítem a frontendet a backendtől jövő eredménnyel. Itt adom meg, hogy mit várok el a backendtől.
            drawItems();
        },
    });
}

/* Dummy
function getShoppingListById(id) {
    console.info(id);

    currentList.name = "Mock Shopping List";
    currentList.items = [
        { name: "Milk" },
        { name: "Apple" },
        { name: "Oranges" }
    ];

    showShoppingList();
    drawItems();
}*/
function getShoppingListById(id) {
    $.ajax({
        type: "GET", // mit csináljon
        dataType: "json", // = send the response as Json (JS object) so we can access its properties.
        url: "api/ShoppingListsEF/" + id, // the controller's address. Entity Framework this time!
        success: function (result) { // callback siker esetén
            currentList = result;
            showShoppingList();
            drawItems();
        },
        error: function () {
            console.error("Error!")
        }
    });
}

function hideShoppingList() {
    $("#createListDiv").show();
    $("#shoppingListDiv").hide();

    $("#shoppingListName").val("");
    // Adding keyboard control:
    $("#shoppingListName").focus(); // Cursor jumps here
    $("#shoppingListName").unbind("keyup"); // To not create a list every time the user (re)visits the first page and hits return.
    $("#shoppingListName").keyup(function (event) {
        if (event.keyCode == 13) {
            createShoppingList();
        }
    }); // if there was a return keypress then createSL()
}

$(document).ready(function () { // when the page loaded

    hideShoppingList();

    var pageUrl = window.location.href;
    var idIndex = pageUrl.indexOf("?id=");
    if (idIndex != -1) {
        getShoppingListById(pageUrl.substring(idIndex + 4));
    }

    window.onpopstate = function (event) { // Whenewer a state was found in the browser history then this event is raised
        if (event.state == null) { // Nincs State
            // hide shopping list
            hideShoppingList();
        }
        else { // Van state, with id
            getShoppingListById(event.state.id);
        }
    };
});