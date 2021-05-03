namespace SubversionToGit
{
    using System;
    using System.IO;
    using System.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            var inputData = new ControlData();
            for (int i = 0; i < args.Length; ++i)
            {
                if (args[i] == "-svn")
                {
                    inputData.SubversionRepository = args[i + 1];
                }
                else if (args[i] == "-dir")
                {
                    inputData.WorkingDirectory = args[i + 1];
                }
                else if (args[i] == "-log")
                {
                    inputData.LogFilePath = args[i + 1];
                }
                else if (args[i] == "-sleep")
                {
                    inputData.SleepMilliseconds = int.Parse(args[i + 1]);
                }
                else if (args[i] == "-authors")
                {
                    inputData.AuthorsPath = args[i + 1];
                }
                else if (args[i] == "--quiet")
                {
                    inputData.Quiet = true;
                }
                else if (args[i] == "--verbose")
                {
                    inputData.Verbose = true;
                }
                else if (args[i] == "--debug")
                {
                    inputData.Debug = true;
                }
                else if (args[i] == "--shortlog")
                {
                    inputData.AbbreviatedLogging = true;
                }
            }

            if (string.IsNullOrEmpty(inputData.SubversionRepository)
                || string.IsNullOrEmpty(inputData.WorkingDirectory)
                || string.IsNullOrEmpty(inputData.AuthorsPath))
            {
                Console.WriteLine("USAGE: SubversionToGit -svn <URL to subversion> -dir <folder where files downloaded> -authors <path to authors file>[ -log <log file path>][ --debug][ -- quiet | --verbose]");
                Console.WriteLine("Hit any key to terminate");
                Console.ReadKey();
                return;
            }

            if (!Directory.Exists(inputData.WorkingDirectory))
            {
                Directory.CreateDirectory(inputData.WorkingDirectory);
            }

            if (string.IsNullOrEmpty(inputData.LogFilePath))
            {
                inputData.LogFilePath = @"C:\temp\SubversionToGit.log";
            }

            Controls.SetData(inputData);

            try
            {
                Logger.WriteNotification("START CONVERSION");
                ExecuteConversion();
                Logger.WriteNotification("FINISHED SUCCESSFULLY");
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }

            Console.WriteLine("Hit any key to terminate");
            Console.ReadKey();
        }

        private static void ExecuteConversion()
        {
            var historyXml = RetrieveRevisions.Execute();

            PrepareInitialState();

            foreach (var logentry in historyXml.Root.Elements().Reverse())
            {
                int revision = int.Parse(logentry.Attribute("revision").Value);
                if (revision <= State.CurrentRevision)
                {
                    continue;
                }

                string author = logentry.Element("author").Value;
                string date = logentry.Element("date").Value;
                string message = logentry.Element("msg").Value;

                CommitRevision.Execute(revision, author, date, message);
            }
        }

        private static void PrepareInitialState()
        {
            State.Load();
            if (State.CurrentRevision < 0)
            {
                GitInitialize.Execute();
            }
        }
    }
}
