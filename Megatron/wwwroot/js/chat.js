"use strict";

const connection = new signalR.HubConnectionBuilder().withUrl("/commentHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    var articleId = document.getElementById("ArticleId").value;
    connection.invoke("SendMessage", user, message)
        .then(
            $.ajax({
                url: '/Article/SendMessage',
                type: 'POST',
                data: {articleId: articleId, message: message},
                success: function (response) {
                    if (!response.success){
                        $('#MessageResult').html('<div class="d-flex flex-row mb-2 alert alert-warning alert-dismissible mt-2"><button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>' +response.message + '</div>');
                    }
                    else{
                        setTimeout(function () {
                            location.reload();
                        },100);
                    }
                },
                error: function () {

                }
            })
        )
        .catch(function (err) {
            return console.error(err.toString());
        });
    event.preventDefault();
});