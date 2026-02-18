using Newtonsoft.Json;
using System.IO;

namespace Client.Logger
{
    public static class LoggerExceptoin
    {
        public static void WriteLogExceptons(Exception ex, object obj)
        {
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logger");
            Directory.CreateDirectory(logFilePath);
            logFilePath = Path.Combine(logFilePath, "ExceptionLog.txt");
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine("----- Exception Object -----");
                writer.WriteLine($"Timestamp: {DateTime.Now}");
                writer.WriteLine($"Message: {ex.Message}");
                writer.WriteLine($"Stack Trace: {ex.StackTrace}");
                writer.WriteLine($"Object: {JsonConvert.SerializeObject(obj, Formatting.Indented)}");
            }
        }
        public static void WriteLogExceptons(Exception ex)
        {
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logger");
            Directory.CreateDirectory(logFilePath);
            logFilePath = Path.Combine(logFilePath, "ExceptionLog.txt");
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
