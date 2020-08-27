using SobrietyApi.Models;
using System.Collections.Generic;

namespace SobrietyApi.Services 
{
    public interface ILeaderboardService
    {
        void AddDailyGoal();
        List<RecordMinimal> Get();
        Record Get(string id);
        Record Create(RecordMinimal recordMinimal);
        void Update(Record recordIn);
        void Remove(Record recordIn);
        void Remove(string id);
    }
}