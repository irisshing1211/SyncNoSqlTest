using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using client.Entities;
using client.Events;
using client.Models;
using client.Repository;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace client.Service
{
    public class SyncService : IHostedService, IDisposable
    {
        private readonly ILogger<SyncService> _logger;
        private readonly SyncSetting _setting;
        private IHttpClientFactory _clientFactory;
        private readonly ClientServerContext _ctx;
        private Timer _timer;

        public SyncService(ILogger<SyncService> logger,
                           IOptions<SyncSetting> setting,
                           ClientServerContext ctx,
                           IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _setting = setting.Value;
            _ctx = ctx;
            _clientFactory = clientFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
            _timer = new Timer(Sync, null, TimeSpan.Zero, TimeSpan.FromMinutes(_setting.SyncDuration));

            return Task.CompletedTask;
        }

        private async void Sync(object state)
        {
            _logger.LogInformation("Timed Hosted Service is working. {Time}", DateTime.Now.ToString());
            var repo = new AccountHistoryRepository(_ctx);
            var syncEvent = new SyncEvent(_clientFactory, $"{_setting.CloudUrl}/AccountSync", repo.LastSync());
            await syncEvent.Push();
            _logger.LogInformation($"{syncEvent.Histories.Count} histories received.");
            await repo.AddHistories(syncEvent.Histories);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose() { _timer?.Dispose(); }
    }
}
