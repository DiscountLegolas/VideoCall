// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const roomname = location.pathname.split('/')[2]
let screenshare = false;
let screenstream;
let users = [];
let screencall;
let username = document.getElementById("username").innerText;
let connection = new signalR.HubConnectionBuilder()
    .withUrl("/rtcHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();
connection.start()
let pid;
let displayframe = document.getElementById('stream__box')
const myPeer = new Peer()
const myVideo = document.createElement('video')
const peers = {}

navigator.mediaDevices.getUserMedia({
    video: true,
    audio: true
}).then(stream => {

    addVideoStream(myVideo, stream, pid)
    myPeer.on('call', call => {
        if (!Array.isArray(peers[call.peer])) {
            peers[call.peer]=[]
        }
        peers[call.peer].push(call)
        call.answer(stream)
        const video = document.createElement('video')
        call.on('stream', userVideoStream => {
            addVideoStream(video, userVideoStream, call.peer)
        })
        call.on('close', () => {
            console.log("fsfs")
            video.remove()
            if (displayframe.getAttribute("data-id") === call.peer) {
                displayframe.children[0].remove();
            }
        })
    })

    connection.on("Greeted", (ausername) => {
        if (!users.includes(ausername)) {
            users.push(ausername)
        }
        addtousers(ausername)
    });
    connection.on("UserJoined", (id, clid, ausername) => {
        if (!users.includes(ausername)) {
            users.push(ausername)
            addtousers(ausername)
            greet(ausername)
            connectToNewUser(id, clid, stream)
            connection.invoke("Greet", username, clid)
                .then(() => {
                })
                .catch((error) => {
                    console.error('Error sending beforeunload signal:', error);
                });
        }
    });
    connection.on("UserLeaved", (id, clid, ausername) => {
        if (peers[id]) {
            removefromusers(ausername)
            for (var i in peers[id]) {
                peers[id][i].close()
            }
        }
        else if (peers[clid]) {
            removefromusers(ausername)
            for (var i in peers[clid]) {
                peers[id][i].close()
            }
        }
        else {
            console.log("error")
        }

    });
    document.getElementById("camera").onclick = ()=>cameramicclick("camera", "video", stream)
    document.getElementById("mic").onclick = () => cameramicclick("mic", "audio", stream)
    document.getElementById("record").onclick = async () => {
        let sst = await navigator.mediaDevices.getDisplayMedia();
        const recorder = new MediaRecorder(sst);

        const chunks = [];
        recorder.ondataavailable = e => chunks.push(e.data);
        recorder.start();
        recorder.onstop = e => {
            const completeBlob = new Blob(chunks, { type: chunks[0].type });
            const downloadLink = document.createElement("a");
            downloadLink.href = URL.createObjectURL(completeBlob);
            downloadLink.download = "screen-recording.webm";
            document.body.appendChild(downloadLink);
            downloadLink.click();
            document.body.removeChild(downloadLink);
        };
    }
    document.getElementById("screen_share").onclick = async () => {
        console.log(screenshare)
        if (!screenshare) {
            screenstream = await navigator.mediaDevices.getDisplayMedia();
            screenshare = !screenshare
            /*
            const screenvideo = document.createElement('video')
            addVideoStream(screenvideo, screenstream, pid)*/
            for (var i in peers) {
                screencall = myPeer.call(i, screenstream)
            }
            screenstream.onended = () => {
                screencall.close()
                screenshare = !screenshare
                document.getElementById("screen_share").style.backgroundColor = "";
                screencall.close()
            };
            document.getElementById("screen_share").style.backgroundColor = "mediumpurple";
        }
        else {
            screencall = !screencall
            document.getElementById("screen_share").style.backgroundColor = "";
            var tracks = screenstream.getTracks();
            for (var i = 0; i < tracks.length; i++) tracks[i].stop();
            screencall.close()
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
myPeer.on('open', id => {
    pid = id;
    users.push(username)
    connection.invoke("JoinRoom", id, roomname, username);
})
window.addEventListener('beforeunload', function (e) {
    e.preventDefault();
    connection.invoke("LeaveRoom", pid, roomname, username)
        .then(() => {
        })
        .catch((error) => {
            console.error('Error sending beforeunload signal:', error);
        });
});
async function connectToNewUser(id, userId, stream) {
    const call = myPeer.call(id, stream)        
    if (screenshare) {
        screencall=myPeer.call(id, screenstream)
    }
    const video = document.createElement('video')
    call.on('stream', userVideoStream => {
        addVideoStream(video, userVideoStream,id)
    })
    call.on('close', () => {
        video.remove()
        if (displayframe.getAttribute("data-id") === id) {
            displayframe.children[0].remove();
        }
    })
    if (!Array.isArray(peers[id])) {
        peers[id] = []
    }
    peers[id].push(call)
}