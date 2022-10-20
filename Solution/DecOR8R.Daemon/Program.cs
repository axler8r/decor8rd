using DecOR8R.Daemon.Service;

var host_ = Host.CreateDefaultBuilder(args)
    .UseSystemd()
    .ConfigureAppConfiguration(
        (
            context,
            builder) =>
        {
            // prepare base path
            var configRoot_ = Environment.GetEnvironmentVariable("$XDG_CONFIG_HOME");
            if (string.IsNullOrEmpty(configRoot_))
                configRoot_ =
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                        ".config");
            var configPath_ = Path.Combine(configRoot_, "decor8r");
            builder.SetBasePath(configPath_);

            // add application configuration files
            var env_ = context.HostingEnvironment;
            builder.AddJsonFile("appsettings.json", false, true);
            builder.AddJsonFile($"appsettings.{env_.EnvironmentName}.json", true, true);

            // add user configuration files
            foreach (var file_ in Directory.GetFiles(
                         configPath_,
                         "*.json",
                         SearchOption.TopDirectoryOnly))
                builder.AddJsonFile(file_, true, true);
        })
    .ConfigureLogging(
        (
            _,
            builder) =>
        {
            builder.AddSystemdConsole(
                options =>
                {
                    options.UseUtcTimestamp = true;
                    options.TimestampFormat = "yyyy-MM-ddTHH:mm:ss.fff ";
                    options.IncludeScopes = true;
                });
            // TODO: Investigate debug logger
            // TODO: Investigate console log formatter
        })
    .ConfigureServices(
        (
            _,
            services) =>
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
