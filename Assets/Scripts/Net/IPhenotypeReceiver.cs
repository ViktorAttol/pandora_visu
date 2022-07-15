using System.Collections.Generic;

namespace Net
{
    public interface IPhenotypeReceiver
    {
        void ReceiveNewPhenotype(List<PhenotypeData> phenotypeData);
    }
}