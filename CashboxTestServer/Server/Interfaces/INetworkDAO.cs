using Server.Modules;

namespace Server.Interfaces
{
    public interface INetworkDAO
    {
        ServerSettings ReadNetworkData();
    }
}
