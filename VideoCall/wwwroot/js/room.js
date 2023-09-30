// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const roomname = location.pathname.split('/')[2]
console.log(roomname)
let connection = new signalR.HubConnectionBuilder()
    .withUrl("/rtcHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();
connection.start()
let pid;
const videoGrid = document.getElementById('videos')
const myPeer = new Peer()
const myVideo = document.createElement('video')
const peers = {}
navigator.mediaDevices.getUserMedia({
    video: true,
    audio: true
}).then(stream => {
    addVideoStream(myVideo, stream)
    myPeer.on('call', call => {
        call.answer(stream)
        const video = document.createElement('video')
        call.on('stream', userVideoStream => {
            console.log("streaming")
            addVideoStream(video, userVideoStream)
        })
    })
    connection.on("UserJoined", (id, clid) => {
        console.log("user joined")
        connectToNewUser(id,clid, stream)
    });
    connection.on("UserLeaved", (id, clid) => {
        if (peers[id]) {
            peers[id].close()
        }
        if (peers[clid]) {
            peers[clid].close()
        }
    });
    document.getElementById("camera").onclick = () => {
        let videotrack = stream.getTracks().find(track => track.kind === 'video')
        videotrack.enabled = !videotrack.enabled;
        if (videotrack.enabled) {
            document.getElementById("camera").style.backgroundColor = "mediumpurple";
        }
        else {
            document.getElementById("camera").style.backgroundColor = "red";
        }
    }
    document.getElementById("mic").onclick = () => {
        let mictrack = stream.getTracks().find(track => track.kind === 'audio')
        mictrack.enabled = !mictrack.enabled;
        if (mictrack.enabled) {
            document.getElementById("mic").style.backgroundColor = "mediumpurple";
        }
        else {
            document.getElementById("mic").style.backgroundColor = "red";
        }
    }
})
window.addEventListener('beforeunload', function (e) {
    e.preventDefault();
    connection.invoke("LeaveRoom", pid, roomname)
        .then(() => {
        })
        .catch((error) => {
            console.error('Error sending beforeunload signal:', error);
        });
});
myPeer.on('open', id => {
    pid = id;
    connection.invoke("JoinRoom", id, roomname);
})
function connectToNewUser(id,userId, stream) {
    const call = myPeer.call(id, stream)
    const video = document.createElement('video')
    call.on('stream', userVideoStream => {
        addVideoStream(video, userVideoStream)
    })
    call.on('close', () => {
        video.remove()
    })

    peers[userId] = call
}

function addVideoStream(video, stream) {
    video.muted = true
    video.setAttribute("class","video-player")
    video.setAttribute("playsinline", "true");
    video.srcObject = stream
    video.addEventListener('loadedmetadata', () => {
        video.play()
    })
    videoGrid.append(video)
}


