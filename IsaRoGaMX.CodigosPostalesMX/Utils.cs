using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsaRoGaMX.CodigosPostalesMX
{
    public class Utils
    {
        public static string GetFullPath(string path)
        {
            // Si la ruta es relativa (no es una ruta absoluta)
            if (!Path.IsPathRooted(path))
            {
                // Combina el directorio actual con la ruta relativa
                return Path.Combine(Directory.GetCurrentDirectory(), path.Replace("./", ""));
            }
            else
            {
                // Si la ruta es absoluta, simplemente la devolvemos tal cual
                return path;
            }
        }
    }
}
