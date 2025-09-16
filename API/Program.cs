using MinimalApi;

IHostBuilder CreateHoustBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    });
}

CreateHoustBuilder(args).Build().Run();