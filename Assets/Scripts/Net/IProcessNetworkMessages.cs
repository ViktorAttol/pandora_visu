using System.Collections.Generic;

namespace Net
{
    public interface IProcessNetworkMessages
    {
        void ReadReceived(ReadData read);

        void PhenotypeReceived(List<PhenotypeData> phenotypeData);

        void StateReceived(DataCollectorState state);

        void ConnectionStatus(NetStatus status);
    }
}