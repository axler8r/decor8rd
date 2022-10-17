using DecOR8R.Daemon.Service;

var host_ = Host.CreateDefaultBuilder(args)
    .UseSystemd()
    .ConfigureAppConfiguration((_, builder) => { })
    .ConfigureLogging((_, builder) =>
    {
        builder.AddSystemdConsole(options =>
        {
            options.UseUtcTimestamp = true;
            options.TimestampFormat = "yyyy-MM-ddTHH:mm:ss.fff ";
            options.IncludeScopes = true;
        });
        // TODO: Investigate debug logger
        // TODO: Investigate console log formatter
    })
    .ConfigureServices((_, services) =>
    {
        services.AddHostedService<Configurator>();
        services.AddHostedService<Listener>();
    })
    .Build();

try
{
    await host_.RunAsync();

    return 0;
}
catch (Exception e_)
{
    Console.Error.WriteLine($"Unable to start service: {e_.Message}");

    return 1;
}
