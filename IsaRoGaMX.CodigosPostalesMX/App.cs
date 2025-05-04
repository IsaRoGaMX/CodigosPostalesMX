using IsaRoGaMX.CodigosPostalesMX.Interfaces;
using IsaRoGaMX.CodigosPostalesMX.Models;

namespace IsaRoGaMX.CodigosPostalesMX
{
    public class App
    {
        private readonly IFetcher _fetcher;
        private readonly IParser _parser;
        private readonly IProcessor _processor;

        // Inyección de dependencias
        public App(IFetcher fetcher, IParser parser, IProcessor processor)
        {
            _fetcher = fetcher;
            _parser = parser;
            _processor = processor;
        }

        public void Run()
        {
            var response = _fetcher.FetchFile();
            if (!response.Success)
                throw new ApplicationException("Error al descargar el archivo");

            DcpRow[] data = _parser.Parse();

            _processor.Process(data);
        }
    }
}
