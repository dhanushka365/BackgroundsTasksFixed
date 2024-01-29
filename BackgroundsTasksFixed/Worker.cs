namespace BackgroundsTasksFixed
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly int _instanceNumber;

        public Worker(ILogger<Worker> logger, int instanceNumber)
        {
            _logger = logger;
            _instanceNumber = instanceNumber;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Worker {_instanceNumber} running at: {DateTimeOffset.Now}");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}