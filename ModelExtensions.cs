using System;
using System.Linq;
using MongoDB.Bson;
using SobrietyApi.Models;

namespace SobrietyApi
{
    public static class ModelExtensions
    {
        public static Record SkipOldAchievements(this Record record)
        {
            return new Record()
            {
                Id = record.Id,
                Name = record.Name,
                Score = record.Score,
                DailyAchievements = new DailyGoal[] 
                {
                    record.DailyAchievements.LastOrDefault()
                }
            };
        }

        public static Record ToRecord(this RecordMinimal recordMinimal)
        {
            return new Record() 
            {
                Name = recordMinimal.Name,
                Score = recordMinimal.Score,
                DailyAchievements = new DailyGoal[]
                {
                    new DailyGoal()
                    {
                        Date = DateTime.Today
                    }
                }
            };
        }
    }
}