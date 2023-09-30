const connection = new signalR.HubConnectionBuilder()
    .withUrl("/rtchub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

async function start() {
    try {
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};
async function JoinRoom(uid, roomname) {
}
function init() {
    start();
}