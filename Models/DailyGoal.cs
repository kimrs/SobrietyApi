using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SobrietyApi.Models
{
    public class DailyGoal
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Date { get; set; }
        public bool NoAlkohol { get; set; }
        public bool NoSnacking { get; set; }
        public bool WorkedOut { get; set; }
    }
}