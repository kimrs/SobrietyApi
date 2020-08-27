using System;
using System.Linq;
using SobrietyApi.Models;

namespace SobrietyApi
{
    public static class ModelExtensions
    {
        public static Record RemoveOldAchievements(this Record record)
        {
            return new Record(record.Score)
            {
                Id = record.Id,
                Name = record.Name,
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