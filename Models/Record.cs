using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq;

namespace SobrietyApi.Models
{
    public class Record
    {
        private int _score;
        public Record(int score = -1)
        {
            _score = score;
        }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set;}
        public string Name { get; set;}

        [BsonIgnore]
        public int Score 
        {
            get {
                var score = _score <= 0 
                ? this.DailyAchievements
                    .SelectMany(a => new bool[]{    a.NoAlkohol,    a.NoSnacking,    a.WorkedOut}) 
                    .Where(point => point)
                    .Count() 
                : _score;
                return score;
            } 
        }
        public DailyGoal[] DailyAchievements { get; set;}
    }

    public class RecordMinimal
    {
        public string Name { get; set;}
        public decimal Score { get; set; }
    }
}