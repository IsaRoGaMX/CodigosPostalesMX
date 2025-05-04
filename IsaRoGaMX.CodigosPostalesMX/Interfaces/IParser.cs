using IsaRoGaMX.CodigosPostalesMX.Models;

namespace IsaRoGaMX.CodigosPostalesMX.Interfaces
{
    public interface IParser
    {
        DcpRow[] Parse();
    }
}
