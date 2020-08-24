
using SobrietyApi.Models;
using MongoDB.Driver;

using System.Collections.Generic;
using System.Linq;

namespace SobrietyApi.Services
{
    public class LeaderboardService
    {
        private readonly IMongoCollection<Record> _records;

        public LeaderboardService(ISoberDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _records = database.GetCollection<Record>(settings.RecordsCollectionName);
        }

        public List<Record> Get() => 
            _records.Find(record => true).ToList();

        public Record Get(string id) 
            => _records.Find<Record>(record => record.Id == id).FirstOrDefault();

        public Record Create(Record record)
        {
            _records.InsertOne(record);
            return record;
        }

        public void Update(Record recordIn) => 
            _records.ReplaceOne(record => record.Id == recordIn.Id, recordIn);

        public void Remove(Record recordIn) =>
            _records.DeleteOne(record => record.Id == recordIn.Id);

        public void Remove(string id) => 
            _records.DeleteOne(record => record.Id == id);
    }
}