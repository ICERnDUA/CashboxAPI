using Newtonsoft.Json;

namespace Server.Logger
{
    public static class Logger
    {
        private static object _locker = new object();
        public static void WriteLog(Exception ex, object obj)
        {

            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logger");
            Directory.CreateDirectory(logFilePath);
            logFilePath = Path.Combine(logFilePath, "ExceptionLog.txt");
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine("----- Exception Object -----");
                writer.WriteLine($"Exception: {ex}");
                writer.WriteLine($"Timestamp: {DateTime.Now}");
                writer.WriteLine($"Message: {ex.Message}");
                writer.WriteLine($"Object: {JsonConvert.SerializeObject(obj, Formatting.Indented)}");
            }
        }
        public static void WriteLog(Exception ex)
        {
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logger");
            Directory.CreateDirectory(logFilePath);
            logFilePath = Path.Combine(logFilePath, "ExceptionLog.txt");
            lock (_locker)
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine("----- Exception -----");
                    writer.WriteLine($"Timestamp: {DateTime.Now}");
                    writer.WriteLine($"Message: {ex.Message}");
                    writer.WriteLine($"Stack Trace: {ex.StackTrace}");

                }
            }
        }
    }
}
