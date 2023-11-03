using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver.Core.Events;
using VideoCall.RepoModel.Repos;
using VideoCall.SignalRtc.Models;

namespace VideoCall.SignalRtc
{
    public class SignalRtcHub: Hub
    {
        private static Dictionary<string, List<string>> groupUsers = new Dictionary<string, List<string>>();
        private readonly RoomRepo _roomRepo;
        public SignalRtcHub(RoomRepo roomRepo)
        {
            _roomRepo = roomRepo;
        }
        public async Task Greet( string username,string clid)
        {
            await Clients.Client(clid).SendAsync("Greeted", username);
        }
        public async Task JoinRoom(string id,string roomname,string username)
        {
            string clid = Context.ConnectionId;
            if (!groupUsers.ContainsKey(roomname))
            {
                groupUsers[roomname] = new List<string>();
            }

            if (!groupUsers[roomname].Contains(username))
            {
                groupUsers[roomname].Add(username);
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, roomname);
            await Clients.GroupExcept(roomname, new List<string> { clid }).SendAsync("UserJoined", id, clid, username);
        }
        public async Task SendingMessage(string roomname, string username,string message)
        {
            await Clients.Group(roomname).SendAsync("ReceiveMessage", username,message);
        }
        public async Task LeaveRoom(string id, string roomname, string username)
        {
            if (groupUsers.ContainsKey(roomname))
            {
                groupUsers[roomname].Remove(username);
            }
            var room = _roomRepo.GetAsync(roomname).Result;
            room.CallHistory.First(x => x.User.UserName == username).EndDate = DateTime.Now;
            await _roomRepo.UpdateAsync(roomname, room);
            string clid = Context.ConnectionId;
            await Groups.RemoveFromGroupAsync(username, roomname);
            await Clients.GroupExcept(roomname, new List<string> { clid }).SendAsync("UserLeaved", id, clid,username);
        }
        public async Task CameraDisabled(string id, string roomname)
        {
            string clid = Context.ConnectionId;
            await Clients.GroupExcept(roomname, new List<string> { clid }).SendAsync("UserCameraDisabled", id, clid);
        }
    }
}
