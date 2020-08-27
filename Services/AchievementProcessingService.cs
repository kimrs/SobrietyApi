using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SobrietyApi.Services
{
    public class AchievementProcessingService : BackgroundService
    {
        private ILeaderboardService _leaderboard;
        private ILogger<AchievementProcessingService> _logger;
        private Timer _timer;

        public AchievementProcessingService(ILogger<AchievementProcessingService> logger, ILeaderboardService leaderboard)
        {
            _logger = logger;
            _leaderboard = leaderboard;
        }

        protected override Task ExecuteAsync(CancellationToken token)
        {
            Task.Run(() => 
            {
                var timeUntilExecution = DateTime.Today.AddDays(1).AddHours(1).Subtract(DateTime.Now);
                _logger.Log(LogLevel.Information, $"Adding new goal in {timeUntilExecution}");
                Task.Delay(timeUntilExecution).Wait();
                _timer = new Timer(x => _leaderboard.AddDailyGoal(), null, TimeSpan.Zero, TimeSpan.FromHours(24));
            });

            return Task.CompletedTask;
        }
    }    
}