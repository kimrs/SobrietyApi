using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SobrietyApi.Services
{
    public class AchievementProcessingService : IHostedService, IDisposable
    {
        private ILeaderboardService _leaderboard;
        private ILogger<AchievementProcessingService> _logger;
        private Timer _timer;

        public AchievementProcessingService(ILogger<AchievementProcessingService> logger, ILeaderboardService leaderboard)
        {
            _logger = logger;
            _leaderboard = leaderboard;
        }

        public Task StartAsync(CancellationToken token)
        {

            var nextRunTime = DateTime.Today.AddDays(1).AddHours(1);
            var timeUntilExecution = nextRunTime.Subtract(DateTime.Now);

            _logger.Log(LogLevel.Information, $"Processing todays achievements in {timeUntilExecution}");
            _leaderboard.ProcessTodaysAchievements();

            Task.Run(() => 
            {
                Task.Delay(timeUntilExecution).Wait();
                _timer = new Timer(x => _leaderboard.ProcessTodaysAchievements(), null, TimeSpan.Zero, TimeSpan.FromHours(24));
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken token) 
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {

        }
    }    
}