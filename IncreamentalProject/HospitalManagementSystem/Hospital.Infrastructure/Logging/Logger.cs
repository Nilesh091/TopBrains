using System;

namespace Hospital.Infrastructure.Logging
{
    public class Logger
    {
        private static readonly string filePath = "errorlog.txt";

        public static void Log(Exception ex)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true)) // true = append mode
                {
                    writer.WriteLine("===========================================");
                    writer.WriteLine($"Date: {DateTime.Now}");
                    writer.WriteLine($"Message: {ex.Message}");
                    writer.WriteLine($"StackTrace: {ex.StackTrace}");
                    writer.WriteLine("===========================================");
                    writer.WriteLine();
                }
            }
            catch
            {
                // Avoid throwing exception inside logger
                // Logging failure should not crash application
            }
        }
    }
}
