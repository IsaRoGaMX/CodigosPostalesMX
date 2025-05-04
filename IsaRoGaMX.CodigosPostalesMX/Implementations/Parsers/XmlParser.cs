using IsaRoGaMX.CodigosPostalesMX.Interfaces;
using IsaRoGaMX.CodigosPostalesMX.Models;
using IsaRoGaMX.CodigosPostalesMX.Settings;
using Microsoft.Extensions.Options;
using System.Xml.Linq;

namespace IsaRoGaMX.CodigosPostalesMX.Implementations.Parsers
{
    public class XmlParser : IParser
    {
        private readonly XmlParserSettings _settings;
        private readonly string _filePath;

        public XmlParser(IOptions<XmlParserSettings> options)
        {
            _settings = options.Value;
            _filePath = Utils.GetFullPath(_settings.XmlFilePath);
        }

        public DcpRow[] Parse()
        {
            Console.WriteLine($"Leyendo archivo: {_filePath}");
            // Cargar el archivo XML
            var xmlDocument = XDocument.Load(_filePath);
            XNamespace ns = "NewDataSet";

            // Obtener los nodos <table>
            return xmlDocument.Descendants(ns + "table")
                .Select(x => new DcpRow
                {
                    DCodigo = x.Element(ns + "d_codigo")?.Value,
                    DAsenta = x.Element(ns + "d_asenta")?.Value,
                    DTipoAsenta = x.Element(ns + "d_tipo_asenta")?.Value,
                    DMnpio = x.Element(ns + "D_mnpio")?.Value,
                    DEstado = x.Element(ns + "d_estado")?.Value,
                    DCiudad = x.Element(ns + "d_ciudad")?.Value,
                    DCP = x.Element(ns + "d_CP")?.Value,
                    CEstado = x.Element(ns + "c_estado")?.Value,
                    COficina = x.Element(ns + "c_oficina")?.Value,
                    CCP = x.Element(ns + "c_CP")?.Value,
                    CTipoAsenta = x.Element(ns + "c_tipo_asenta")?.Value,
                    CMnpio = x.Element(ns + "c_mnpio")?.Value,
                    IdAsentaCpCons = x.Element(ns + "id_asenta_cpcons")?.Value,
                    DZona = x.Element(ns + "d_zona")?.Value,
                    CCveCiudad = x.Element(ns + "c_cve_ciudad")?.Value
                })
                .ToArray();
        }
    }
}
