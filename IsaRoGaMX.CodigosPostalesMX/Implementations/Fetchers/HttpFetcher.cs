using IsaRoGaMX.CodigosPostalesMX.Interfaces;
using IsaRoGaMX.CodigosPostalesMX.Models;
using IsaRoGaMX.CodigosPostalesMX.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsaRoGaMX.CodigosPostalesMX.Implementations.Fetchers
{
    public class HttpFetcher : IFetcher
    {
        private readonly HttpFetcherSettings _settings;
        private string _zipFullPath;
        private string _extractedFileFullPath;

        public HttpFetcher(IOptions<HttpFetcherSettings> options)
        {
            _settings = options.Value;
            _zipFullPath = Path.Combine(Utils.GetFullPath(_settings.BasePath), _settings.ZipFileName);
            _extractedFileFullPath = Path.Combine(Utils.GetFullPath(_settings.BasePath), _settings.ExtractedFileName);
        }

        public FetcherResult FetchFile()
        {
            try
            {
                if (File.Exists(_zipFullPath))
                {
                    Console.WriteLine($"Eliminando archivo: {_zipFullPath}");
                    File.Delete(_zipFullPath);
                }

                if (File.Exists(_extractedFileFullPath))
                {
                    Console.WriteLine($"Eliminando archivo: {_extractedFileFullPath}");
                    File.Delete(_extractedFileFullPath);
                }

                var client = new HttpClient();

                var formData = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("__EVENTTARGET", ""),
                new KeyValuePair<string, string>("__EVENTARGUMENT", ""),
                new KeyValuePair<string, string>("__LASTFOCUS", ""),
                new KeyValuePair<string, string>("__VIEWSTATE",
                    "/wEPDwUINzcwOTQyOTgPZBYCAgEPZBYCAgEPZBYGAgMPDxYCHgRUZXh0BTjDmmx0aW1hIEFjdHVhbGl6YWNpw7NuIGRlIEluZm9ybWFjacOzbjogRW5lcm8gMjcgZGUgMjAyNWRkAgcPEA8WBh4NRGF0YVRleHRGaWVsZAUDRWRvHg5EYXRhVmFsdWVGaWVsZAUFSWRFZG8eC18hRGF0YUJvdW5kZ2QQFSEjLS0tLS0tLS0tLSBUICBvICBkICBvICBzIC0tLS0tLS0tLS0OQWd1YXNjYWxpZW50ZXMPQmFqYSBDYWxpZm9ybmlhE0JhamEgQ2FsaWZvcm5pYSBTdXIIQ2FtcGVjaGUUQ29haHVpbGEgZGUgWmFyYWdvemEGQ29saW1hB0NoaWFwYXMJQ2hpaHVhaHVhEUNpdWRhZCBkZSBNw6l4aWNvB0R1cmFuZ28KR3VhbmFqdWF0bwhHdWVycmVybwdIaWRhbGdvB0phbGlzY28HTcOpeGljbxRNaWNob2Fjw6FuIGRlIE9jYW1wbwdNb3JlbG9zB05heWFyaXQLTnVldm8gTGXDs24GT2F4YWNhBlB1ZWJsYQpRdWVyw6l0YXJvDFF1aW50YW5hIFJvbxBTYW4gTHVpcyBQb3Rvc8OtB1NpbmFsb2EGU29ub3JhB1RhYmFzY28KVGFtYXVsaXBhcwhUbGF4Y2FsYR9WZXJhY3J1eiBkZSBJZ25hY2lvIGRlIGxhIExsYXZlCFl1Y2F0w6FuCVphY2F0ZWNhcxUhAjAwAjAxAjAyAjAzAjA0AjA1AjA2AjA3AjA4AjA5AjEwAjExAjEyAjEzAjE0AjE1AjE2AjE3AjE4AjE5AjIwAjIxAjIyAjIzAjI0AjI1AjI2AjI3AjI4AjI5AjMwAjMxAjMyFCsDIWdnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2dnZ2RkAh0PPCsACwBkGAEFHl9fQ29udHJvbHNSZXF1aXJlUG9zdEJhY2tLZXlfXxYBBQtidG5EZXNjYXJnYZRWX+lZqIiBEYizlI9sK5M6gc7/"),
                new KeyValuePair<string, string>("__VIEWSTATEGENERATOR", "BE1A6D2E"),
                new KeyValuePair<string, string>("__EVENTVALIDATION",
                    "/wEWKALl/fBMAsb84u8GAtaTiIIKAtaThIIKAtaTgIIKAtaTvIIKAtaTuIIKAtaTtIIKAtaTsIIKAtaTrIIKAtaT6IEKAtaT5IEKAsmTiIIKAsmThIIKAsmTgIIKAsmTvIIKAsmTuIIKAsmTtIIKAsmTsIIKAsmTrIIKAsmT6IEKAsmT5IEKAsiTiIIKAsiThIIKAsiTgIIKAsiTvIIKAsiTuIIKAsiTtIIKAsiTsIIKAsiTrIIKAsiT6IEKAsiT5IEKAsuTiIIKAsuThIIKAsuTgIIKAsv65NYEAtrhlrgCAr6o7JEBAsjnpvoLAvX8qO0FhYjIz04JbmP0n1MQwbOmKGFUhyY="),
                new KeyValuePair<string, string>("cboEdo", "00"),
                new KeyValuePair<string, string>("rblTipo", "xml"),
                new KeyValuePair<string, string>("btnDescarga.x", "11"),
                new KeyValuePair<string, string>("btnDescarga.y", "11")
            });

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(_settings.Url),
                    Content = formData
                };

                Console.WriteLine("Descargando archivo...");
                using (var response = client.SendAsync(request).GetAwaiter().GetResult())
                {
                    response.EnsureSuccessStatusCode();
                    var fileContent = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                    File.WriteAllBytes(_zipFullPath, fileContent);
                    Console.WriteLine($"Archivo Zip Descargado: {_zipFullPath}");
                }

                ZipFile.ExtractToDirectory(_zipFullPath, Directory.GetCurrentDirectory());
                Console.WriteLine($"Archivo Descomprimido: {_extractedFileFullPath}");

                return new FetcherResult(true, Path.Combine(Utils.GetFullPath(_settings.BasePath), "CPdescarga.xml"));
            }
            catch (Exception ex)
            {
                return new FetcherResult(false, exception: ex);
            }
        }
    }
}
