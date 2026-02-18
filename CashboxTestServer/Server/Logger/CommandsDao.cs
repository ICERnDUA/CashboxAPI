using CashboxCommands.Converters;
using CashboxCommands.Data;
using Newtonsoft.Json;
using Server.Interfaces;
using Server.Modules;
using System.IO;
using static Server.Program;

namespace Server.Logger
{
    public class CommandsDao
    {

        INetworkDAO _networkDAO;
        ServerSettings _serverSettings;
        string _directoryPath;
        object _locker = new object();
        public CommandsDao(INetworkDAO networkDAO)
        {
            _networkDAO = networkDAO;
            _serverSettings = _networkDAO.ReadNetworkData();
            CheckDirectory();
        }
        public void WriteCommands(BaseCommand Command)
        {
            try
            {
                CheckDirectory();
                string StringCommand = JsonConvert.SerializeObject(Command, new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });
                lock (_locker)
                {
                    if (_serverSettings != null && _serverSettings.DatabasePath != null)
                    {
                        using (StreamWriter w = File.AppendText(Path.Combine(_directoryPath, "result.json")))
                        {
                            w.WriteLine(StringCommand + ",");
                        }
                    }

                    else
                    {
                        using (StreamWriter w = File.AppendText(Path.Combine(_directoryPath, "result.json")))
                        {
                            w.WriteLine(StringCommand + ",");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteLog(ex);
            }
        }
        private void CheckDirectory()
        {

            if (_serverSettings != null && _serverSettings.DatabasePath != null)
            {
                _directoryPath = Path.Combine(_serverSettings.DatabasePath, $"{DateTime.Today.ToShortDateString()}");
                if (!Directory.Exists(_directoryPath)) 
                    Directory.CreateDirectory(_directoryPath);
            }
            else
            {
                _directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DB", $"{DateTime.Today.ToShortDateString()}");
                if (!Directory.Exists(_directoryPath));
                    Directory.CreateDirectory(_directoryPath);
            }
        }
    }
}
