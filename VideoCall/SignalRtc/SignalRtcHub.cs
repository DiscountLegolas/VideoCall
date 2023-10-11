using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using VideoCall.SignalRtc.Models;

namespace VideoCall.SignalRtc
{
    public class SignalRtcHub: Hub
    {
        public async Task Greet( string username,string clid)
        {
            await Clients.Client(clid).SendAsync("Greeted", username);
        }
        public async Task JoinRoom(string id,string roomname,string username)
        {
            string clid = Context.ConnectionId;
            await Groups.AddToGroupAsync(Context.ConnectionId, roomname);
            await Clients.GroupExcept(roomname, new List<string> { clid }).SendAsync("UserJoined",id, clid, username);
        }
        public async Task ExpandFrame(string roomname,string id)
        {
            string clid = Context.ConnectionId;
            await Clients.GroupExcept(roomname, new List<string> { clid }).SendAsync("UserExpanded",id);
        }
        public async Task LeaveRoom(string id, string roomname, string username)
        {
            string clid = Context.ConnectionId;
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomname);
            await Clients.GroupExcept(roomname, new List<string> { clid }).SendAsync("UserLeaved", id, clid,username);
        }
        public async Task CameraDisabled(string id, string roomname)
        {
            string clid = Context.ConnectionId;
            await Clients.GroupExcept(roomname, new List<string> { clid }).SendAsync("UserCameraDisabled", id, clid);
        }
    }
}
