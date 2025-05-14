using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.BackService
{
    public class Service1: IHostedService
    {
        private Timer? _timer;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        
        public Service1(ILogger<Service1> logger)
        {
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(_timer_Callback, 
                null,
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(1)
                );
           return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken) {
           _timer?.Dispose();
            return Task.CompletedTask;
            }
        private void _timer_Callback(object? state)
        {
            _logger.LogInformation("Timer Callback from Service");
        } 
    }
}
