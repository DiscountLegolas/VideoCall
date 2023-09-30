using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using VideoCall.SignalRtc.Models;

namespace VideoCall.SignalRtc
{
    public class SignalRtcHub: Hub
    {
        public async Task JoinRoom(string id,string roomname)
        {
            Console.WriteLine(roomname);
            string clid = Context.ConnectionId;
            await Groups.AddToGroupAsync(Context.ConnectionId, roomname);
            await Clients.OthersInGroup(roomname).SendAsync("UserJoined",id, clid);
        }
        public async Task LeaveRoom(string id, string roomname)
        {
            Console.WriteLine(roomname);
            string clid = Context.ConnectionId;
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomname);
            await Clients.OthersInGroup(roomname).SendAsync("UserLeaved", id, clid);
        }
    }
}
