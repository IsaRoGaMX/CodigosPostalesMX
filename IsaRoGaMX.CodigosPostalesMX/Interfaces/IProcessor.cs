using IsaRoGaMX.CodigosPostalesMX.Models;

namespace IsaRoGaMX.CodigosPostalesMX.Interfaces
{
    public interface IProcessor
    {
        void Process(DcpRow[] registro);
    }
}
