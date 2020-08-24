using System;
using System.Linq;
using System.Collections.Generic;

using SobrietyApi.Models;
using MongoDB.Driver;


namespace SobrietyApi.Services
{
    public interface ILeaderboardService
    {
        void ProcessTodaysAchievements();
    }

    public class LeaderboardService : ILeaderboardService
    {
        private readonly IMongoCollection<Record> _records;

        public LeaderboardService(ISoberDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _records = database.GetCollection<Record>(settings.RecordsCollectionName);
        }


        public void ProcessTodaysAchievements()
        {
            var records = _records.Find<Record>(record => true).ToList();
            foreach(var record in records)
            {
                record.Score = record.DailyAchievements
                    .SelectMany(achivement => new bool[]
                    {
                        achivement.NoAlkohol, 
                        achivement.NoSnacking, 
                        achivement.WorkedOut 
                    })
                    .Where(point => point)
                    .Count();
                record.DailyAchievements.ToList().Add( new DailyGoal() { Date = DateTime.Today } );

                _records.ReplaceOne(r => r.Id == record.Id, record);
            }
        }

        public List<RecordMinimal> Get() 
            => _records.Find(record => true)
            .Project(p => new RecordMinimal { Name = p.Name, Score = p.Score})
            .ToList();

        public Record Get(string id) 
            => _records.Find<Record>(record => record.Id == id)
            .Project(x => x.SkipOldAchievements())
            .FirstOrDefault();

        public Record Create(RecordMinimal recordMinimal)
        {
            var record = recordMinimal.ToRecord();
            _records.InsertOne(record);

            return record;
        }

        public void Update(Record recordIn) 
        {
            _records.ReplaceOne(record => record.Id == recordIn.Id, recordIn);
        }

        public void Remove(Record recordIn) =>
            _records.DeleteOne(record => record.Id == recordIn.Id);

        public void Remove(string id) => 
            _records.DeleteOne(record => record.Id == id);
    }
}