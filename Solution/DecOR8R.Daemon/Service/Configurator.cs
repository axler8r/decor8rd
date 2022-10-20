namespace DecOR8R.Daemon.Service;

public class Configurator : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<Configurator> _logger;

    public Configurator(ILogger<Configurator> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);

            _logger.LogInformation("Configurator running at: {Now}", DateTimeOffset.UtcNow.ToString("O"));
            _logger.LogDebug("Configuration is {Configuration}", _configuration.ToString());
            _logger.LogDebug("Base00 is {Base00}", _configuration["scheme:base00"]);
        }
    }
}
