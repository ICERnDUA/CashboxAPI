using Newtonsoft.Json;
using Server.Interfaces;

namespace Server.Modules
{
    public class NetworkDAO: INetworkDAO
    {
        public ServerSettings ReadNetworkData()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Settings.json");
            if (File.Exists(filePath))
            {
                return JsonConvert.DeserializeObject<ServerSettings>(File.ReadAllText(filePath));

            }
            else
            {
                
                return null;

            }

        }      
    }
}
