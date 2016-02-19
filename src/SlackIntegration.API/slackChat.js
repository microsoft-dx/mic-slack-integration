$.ajax({
    url: '/api/Slack/GetMessages',
    method: 'get',
    data: { messagesCount: 30 },
    contentType: 'json',
    success: addMessages
});

function addMessages(messageList) {
    for (var index = messageList.length - 1; index => 0; index--) {
        var message = messageList[index];
        $("#messageList").append('<li>' + '<b>' + message.username + '</b>:' + message.text + '</li>');

    }
}



var hub = $.connection.slackHub;

hub.client.addMessage = function (userName, message) {
    $("#messageList").append('<li>' + '<b>' + userName + '</b>:' + message + '</li>');
};

$("#sendMessageButton").click(function () {

    var payload = {
        UserName: $("#userNameInput").val() || "Guest",
        Text: $("#messageInput").val()
    };
    $.ajax({
        url: '/api/Slack/PostMessageToSlack',
        method: 'POST',
        dataType: 'json',
        data: payload
    });
});

$.connection.hub.logging = true;
$.connection.hub.start();