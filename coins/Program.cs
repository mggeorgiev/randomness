using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();
