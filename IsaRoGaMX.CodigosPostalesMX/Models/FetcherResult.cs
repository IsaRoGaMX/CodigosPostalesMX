using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsaRoGaMX.CodigosPostalesMX.Models
{
    public class FetcherResult(bool success, string downloadedFilePath = "", Exception exception = null)
    {
        public bool Success { get; } = success;
        public string DownloadedFilePath { get; } = downloadedFilePath;
        public Exception Exception { get; } = exception;
    }
}
