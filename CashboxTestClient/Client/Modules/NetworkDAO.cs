using Newtonsoft.Json;
using Client.Interfaces;
using System.IO;

namespace Client.Models
{
    public class NetworkDAO: INetworkDAO
    {
        public NetworkConnections ReadNetworkData()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Settings.json");
            if (File.Exists(filePath))
            {
                return JsonConvert.DeserializeObject<NetworkConnections>(File.ReadAllText(filePath));

            }
            else
            {
                //var baseNetworkData = new NetworkConnections() { ipAddress = "localhost", port = 5000 };
                //string json = JsonConvert.SerializeObject(baseNetworkData);
                //File.WriteAllText(filePath, json);
                return null;

            }

        }

        public void SaveNetworkData(NetworkConnections networkConnections)
        {
            string json = JsonConvert.SerializeObject(networkConnections);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Settings.json");
            File.WriteAllText(filePath, json);

        }
    }
}
