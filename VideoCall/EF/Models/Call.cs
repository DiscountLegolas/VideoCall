using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using VideoCall.Identity.Models;

namespace VideoCall.EF.Models
{
    public class Call
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
