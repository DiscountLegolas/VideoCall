let expandvideoframe = (e, id) => {
    displayframe.innerHTML = "";
    displayframe.style.display = "block";
    displayframe.setAttribute("data-id", id)
    let videotoexpand = e.currentTarget.cloneNode(true);
    videotoexpand.style.height = "auto"
    videotoexpand.style.width = "100%"
    videotoexpand.style.margin= "0"

    videotoexpand.srcObject = e.currentTarget.srcObject;
    videotoexpand.addEventListener('loadedmetadata', () => {

        videotoexpand.play()
    })
    displayframe.appendChild(videotoexpand)
}
function cameramicclick(id,type,stream) {
    let videotrack = stream.getTracks().find(track => track.kind === type)
    videotrack.enabled = !videotrack.enabled;
    if (videotrack.enabled) {
        document.getElementById(id).style.backgroundColor = "mediumpurple";
    }
    else {
        document.getElementById(id).style.backgroundColor = "";
    }
}
