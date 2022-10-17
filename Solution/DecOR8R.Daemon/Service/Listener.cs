using System.Net.Sockets;

namespace DecOR8R.Daemon.Service;

public class Listener : BackgroundService
{
    private readonly ILogger<Listener> _logger;
    private readonly IConfiguration _configuration;
    private readonly string _socketFileName;

    public Listener(ILogger<Listener> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _socketFileName = Path.Combine(Path.GetTempPath(), "decor8rd.sock");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting {This}", GetType().FullName);

        _logger.LogTrace("Preparing UNIX domain socket: {SocketFileName}", _socketFileName);
        if (File.Exists(_socketFileName)) File.Delete(_socketFileName);
        var endpoint_ = new UnixDomainSocketEndPoint(_socketFileName);

        _logger.LogInformation("Listening for incoming connections");
        using var listener_ = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
        listener_.Bind(endpoint_);
        listener_.Listen(1);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(0, stoppingToken);

            _logger.LogInformation("Accepting request {Now}", DateTime.UtcNow.ToString("O"));
            using var socket_ = await listener_.AcceptAsync(stoppingToken);

            _logger.LogTrace("Processing request");
            await using var stream_ = new NetworkStream(socket_);
            using var reader_ = new StreamReader(stream_);
            var request_ = await reader_.ReadLineAsync();

            _logger.LogTrace("Request: {Request}", request_);
            // for each segment in the configuration
            // pass the request to the segment processor
            // collect the processed requests
            // pass it to renderers
            // return the consolidated result to the client

            await using var writer_ = new StreamWriter(stream_);
            await writer_.WriteLineAsync(request_);
            await writer_.FlushAsync();
            _logger.LogInformation("Request processed");
        }
    }
}
