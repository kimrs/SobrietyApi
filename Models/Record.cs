using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SobrietyApi.Models
{
    public class Record
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set;}
        public string Name { get; set;}
        public decimal Score { get; set; }
        public DailyGoal[] DailyAchievements { get; set;}
    }

    public class RecordMinimal
    {
        public string Name { get; set;}
        public decimal Score { get; set; }
    }
}