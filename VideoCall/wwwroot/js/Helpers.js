function addVideoStream(video, stream, id) {
    const audioContext = new (window.AudioContext || window.webkitAudioContext)();
    const analyser = audioContext.createAnalyser();
    const microphoneSource = audioContext.createMediaStreamSource(stream);

    microphoneSource.connect(analyser);
    analyser.connect(audioContext.destination);

    analyser.fftSize = 256; // Set the FFT (Fast Fourier Transform) size
    const bufferLength = analyser.frequencyBinCount;
    const dataArray = new Uint8Array(bufferLength);

    function updateAudioLevel() {
        analyser.getByteFrequencyData(dataArray);
        const audioLevel = getAverage(dataArray); // Calculate the average audio level

        // Use audioLevel as needed
        console.log('Audio Level:', audioLevel);
        if (audioLevel > 50) {
            video.setAttribute("class", "video-player-talking")
        }
        else {
            video.setAttribute("class", "video-player")
        }
        // Continue to update audio level as needed
        requestAnimationFrame(updateAudioLevel);
    }

    function getAverage(array) {
        let sum = 0;
        for (let i = 0; i < array.length; i++) {
            sum += array[i];
        }
        return sum / array.length;
    }

    updateAudioLevel();
    const videoGrid = document.getElementById('streams__container')
    video.muted = true

    video.setAttribute("class", "video-player")
    video.setAttribute("data-id", `${id}`)
    video.setAttribute("playsinline", "true");
    video.srcObject = stream
    video.addEventListener('loadedmetadata', () => {

        video.play()
    })
    if (id !== "undefined") {
        video.addEventListener("click", (e) => {
            expandvideoframe(e, id)
        });
    }

    videoGrid.append(video)
}
function addtousers(username) {
    console.log(username)
    var memberlist = document.getElementById("member__list");
    memberlist.innerHTML +=
        `
        <div class="member__wrapper" id="member__1__wrapper" data-username=${username}>
          <p class="member_name">${username}</p>
        </div>
    `
    var count = document.getElementById("members__count");
    count.innerHTML = Number(count.innerText)+1
}
function removefromusers(username) {
    const index = users.indexOf(username);
    if (index > -1) { // only splice array when item is found
        users.splice(index, 1); // 2nd parameter means remove one item only
    }

    var memberlist = document.getElementById("member__list");
    const element = document.querySelector(`[data-username=${username}]`);
    memberlist.removeChild(element)
    var count = document.getElementById("members__count");
    count.innerHTML = Number(count.innerText) - 1
}
function greet(username) {
    var messages = document.getElementById("messages");
    console.log(messages)
    messages.innerHTML += `
                    <div class="message__wrapper">
                    <div class="message__body__bot">
                        <strong class="message__author__bot">🤖 Vidro Bot</strong>
                        <p class="message__text__bot">${username} just entered the room!</p>
                    </div>
                </div>

    `
}
