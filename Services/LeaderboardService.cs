using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SobrietyApi.Models;
using MongoDB.Driver;

namespace SobrietyApi.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly IMongoCollection<Record> _records;
        private readonly ILogger<ILeaderboardService> _logger;

        public LeaderboardService(ISoberDatabaseSettings settings, ILogger<ILeaderboardService> logger)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _records = database.GetCollection<Record>(settings.RecordsCollectionName);
            _logger = logger;
        }

        public void AddDailyGoal()
            => _records.UpdateMany(r => true, Builders<Record>.Update.AddToSet("DailyAchievements", new DailyGoal() { Date = DateTime.Today }));

        private static int ComputeScore(Record record) 
            => record.DailyAchievements 
            .SelectMany(a => new bool[]
            {
                a.NoAlkohol, 
                a.NoSnacking, 
                a.WorkedOut 
            })
            .Where(point => point)
            .Count();

        public List<RecordMinimal> Get() 
            => _records.Find(record => true)
            .Project(p => new RecordMinimal { Name = p.Name, Score = p.Score })
            .ToList();

        public Record Get(string id) 
            => _records.Find<Record>(record => record.Id == id)
            .Project(x => x.RemoveOldAchievements())
            .FirstOrDefault();

        public void Update(Record recordIn) =>
            _records.ReplaceOne(record => record.Id == recordIn.Id, recordIn);

        public void Remove(Record recordIn) =>
            _records.DeleteOne(record => record.Id == recordIn.Id);

        public void Remove(string id) => 
            _records.DeleteOne(record => record.Id == id);

        public Record Create(RecordMinimal recordMinimal)
        {
            var record = recordMinimal.ToRecord();
            _records.InsertOne(record);

            return record;
        }
    }
}