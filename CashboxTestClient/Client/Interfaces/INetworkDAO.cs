using Client.Models;

namespace Client.Interfaces
{
    public interface INetworkDAO
    {
        NetworkConnections ReadNetworkData();
        void SaveNetworkData(NetworkConnections networkConnections);
    }
}
