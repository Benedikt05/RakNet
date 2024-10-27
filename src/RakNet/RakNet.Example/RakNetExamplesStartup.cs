using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RakNet.Example;

if (args.Length == 0) return;

var hosting = Host.CreateDefaultBuilder().ConfigureServices((_, services) =>
{
    var serviceId = args[0].ToLower();
    switch (serviceId)
    {
        case RakNetServerExample.ServiceId:
            services.AddHostedService<RakNetServerExample>();
            break;
        case RakNetClientExample.ServiceId:
            services.AddHostedService<RakNetClientExample>();
            break;
        default: throw new InvalidOperationException($"Unknown service id: {serviceId}");
    }
}).Build();

await hosting.RunAsync();