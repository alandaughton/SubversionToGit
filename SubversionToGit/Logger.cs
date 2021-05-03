namespace SubversionToGit
{
    using System;
    using System.IO;

    static class Logger
    {
        private const string format_iso = "yyyy-MM-ddTHH:mm:ss.fff";

        private static int revisionLoopCount = 0;
        private static readonly object loggerLock = new object();

        static Logger()
        {
            if (!File.Exists(Controls.LogFilePath))
            {
                using (var writer = File.CreateText(Controls.LogFilePath)) { }
            }
        }

        public static void WriteNotification(string message)
        {
            lock (loggerLock)
            {
                if (!Controls.Quiet)
                {
                    WriteToConsole(message);
                }

                if (!Controls.AbbreviatedLogging)
                {
                    WriteToLogFile(message);
                }
            }
        }

        public static void WriteInfo(string message)
        {
            lock (loggerLock)
            {
                if (!Controls.Quiet && Controls.Verbose)
                {
                    WriteToConsole(message);
                }

                if (!Controls.AbbreviatedLogging)
                {
                    WriteToLogFile(message);
                }
            }
        }

        public static void WriteError(Exception ex)
        {
            lock (loggerLock)
            {
                if (!Controls.Quiet)
                {
                    WriteToConsole("ERROR: " + ex.Message);
                }

                WriteToLogFile(ex.ToString());
            }
        }

        public static void WriteStartRevision(string message)
        {
            if (!Controls.AbbreviatedLogging)
            {
                WriteNotification(message);
            }
            else
            {
                lock (loggerLock)
                {
                    if (!Controls.Quiet)
                    {
                        WriteToConsole(message);
                    }

                    if (revisionLoopCount++ % 1000 == 0)
                    {
                        WriteToLogFile(message);
                    }
                }
            }
        }

        private static void WriteToConsole(string message)
        {
            Console.WriteLine(message);
        }

        private static void WriteToLogFile(string message)
        {
            int count = 0;
            while (true)
            {
                try
                {
                    File.AppendAllLines(Controls.LogFilePath, new string[] { AppendTimestamp(message) });
                    break;
                }
                catch
                {
                    if (++count >= 5)
                    {
                        throw;
                    }
                }
            }
        }

        private static string AppendTimestamp(string message)
        {
            return message + $" [TS:{DateTime.Now.ToString(format_iso)}]";
        }
    }
}
