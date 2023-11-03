using Microsoft.Extensions.Options;
using MongoDB.Driver;
using VideoCall.EF.Models;
using VideoCall.Settings;

namespace VideoCall.RepoModel.Repos
{
    public class RoomRepo
    {
        private readonly IMongoCollection<Room> _roomCollection;
        public RoomRepo(IOptions<VidroMongoDbConfig> vidroStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
            vidroStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                vidroStoreDatabaseSettings.Value.DatabaseName);

            _roomCollection = mongoDatabase.GetCollection<Room>("Rooms");
        }
        public async Task<List<Room>> GetAsync() =>
        await _roomCollection.Find(_ => true).ToListAsync();

        public async Task<Room?> GetAsync(string id) =>
            await _roomCollection.Find(x => x.RoomId.Equals(id)).FirstOrDefaultAsync();

        public async Task CreateAsync(Room newRoom) =>
            await _roomCollection.InsertOneAsync(newRoom);

        public async Task UpdateAsync(string id, Room updatedroom) =>
            await _roomCollection.ReplaceOneAsync(x => x.RoomId.Equals(id), updatedroom);

        public async Task RemoveAsync(string id) =>
            await _roomCollection.DeleteOneAsync(x => x.RoomId.ToString() == id);
    }
}
