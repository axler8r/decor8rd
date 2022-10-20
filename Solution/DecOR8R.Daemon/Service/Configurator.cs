using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace DecOR8R.Daemon.Service;

public class Configurator : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<Configurator> _logger;

    public Configurator(
        ILogger<Configurator> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }
}
