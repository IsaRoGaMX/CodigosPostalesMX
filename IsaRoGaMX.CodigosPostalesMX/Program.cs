using IsaRoGaMX.CodigosPostalesMX.Implementations.Fetchers;
using IsaRoGaMX.CodigosPostalesMX.Implementations.Parsers;
using IsaRoGaMX.CodigosPostalesMX.Implementations.Processors;
using IsaRoGaMX.CodigosPostalesMX.Interfaces;
using IsaRoGaMX.CodigosPostalesMX.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IsaRoGaMX.CodigosPostalesMX
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();

            var serviceProvider = new ServiceCollection()
            .Configure<HttpFetcherSettings>(configuration.GetSection("HttpFetcher"))
            .Configure<XmlParserSettings>(configuration.GetSection("XmlParser"))
            .Configure<SqlProcessorSettings>(configuration.GetSection("SqlProcessor"))
            .AddSingleton<IFetcher, HttpFetcher>()
            .AddSingleton<IParser, XmlParser>()
            .AddSingleton<IProcessor, SqlProcessor>()
            .AddSingleton<App>() // Registra la clase App
            .BuildServiceProvider();

            // Resolver la clase App
            var app = serviceProvider.GetRequiredService<App>();

            // Ejecutar la aplicación
            app.Run();

            Console.ReadLine();
        }
    }
}
