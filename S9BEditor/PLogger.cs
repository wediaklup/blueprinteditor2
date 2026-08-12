using System;
using System.IO;
using System.Threading;

namespace S9BEditor
{
    public static class PLogger
    {
        private static readonly object _lock = new object();

        private static readonly string LogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PLogger.log");

        public static void Write(string message)
        {
            try
            {
                lock (_lock)
                {
                    File.AppendAllText(
                        LogFile,
                        string.Format(
                            "{0:yyyy-MM-dd HH:mm:ss.fff} [T{1}] {2}{3}",
                            DateTime.Now,
                            Thread.CurrentThread.ManagedThreadId,
                            message,
                            Environment.NewLine));
                }
            }
            catch
            {
                // Der Logger darf niemals selbst die Anwendung zum Absturz bringen.
                
                // ok? - machen wir so
            }
        }
    }
}