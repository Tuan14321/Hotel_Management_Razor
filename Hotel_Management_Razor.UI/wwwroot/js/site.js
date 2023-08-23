
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/signalRServer")
    .build();

connection.on("Load", function () {
    //alert('d')
     location.href = '/'
   // window.location.reload();
});
connection.start().catch(function (err) {
    return console.error(err.toString());
});
