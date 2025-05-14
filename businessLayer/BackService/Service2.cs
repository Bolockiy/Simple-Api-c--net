using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLayer.BackService
{
    public class Service2 : BackgroundService
    {
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        private Timer? _timer;
        public Service2(ILogger<Service2> logger)
        {
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(_timer_Callback,
             null,
             TimeSpan.FromSeconds(1),
             TimeSpan.FromSeconds(1)
             );
            return Task.CompletedTask;
        }
        private void _timer_Callback(object? state)
        {
            _logger.LogInformation("Timer Callback from Service2");
        }
    }
}
