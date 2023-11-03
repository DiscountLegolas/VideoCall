using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using VideoCall.PageModels;
using VideoCall.Identity.Models;

namespace VideoCall.EF.Models
{
    public class Room
    {
        [BsonId]
        public Guid RoomId { get; set; }
        public string Topic { get; set; }
        public int MaxParicipants {  get; set; }
        public string BeginDateTime { get; set; }
        public string EndDateTime { get; set; }
        public ApplicationUser CreatorUser { get; set; }
        public string CreatedAt { get; set; }
        public ICollection<Call> CallHistory { get; set; }
    }
}
