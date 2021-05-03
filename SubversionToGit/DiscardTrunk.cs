namespace SubversionToGit
{
    using System;
    using System.IO;
    using System.Threading;

    static class DiscardTrunk
    {
        public static void Execute()
        {
            Logger.WriteInfo("Lifting files out of the trunk");

            string tempPath = null;
            try
            {
                string trunkPath = Path.Combine(Controls.WorkingDirectory, "trunk");

                tempPath = Path.GetTempFileName();
                var command = DirectoryCommand.ListFilesAndFolders(trunkPath, tempPath);
                Shell.Execute(command);

                string renamedTrunkSubDirectory = null;
                using (var reader = File.OpenText(tempPath))
                {
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        string currentLine = line;
                        line = reader.ReadLine();

                        string path = Path.Combine(trunkPath, currentLine);
                        if (File.Exists(path))
                        {
                            File.Move(path, Path.Combine(Controls.WorkingDirectory, currentLine));
                        }
                        else if (Directory.Exists(path))
                        {
                            if (string.Compare(currentLine, "trunk", ignoreCase: true) == 0)
                            {
                                var trunkSuffix = Guid.NewGuid().ToString("D");
                                renamedTrunkSubDirectory = $"{currentLine}_{trunkSuffix}"; // adds 33 characters to the path
                                string newTrunkPath = Path.Combine(Controls.WorkingDirectory, renamedTrunkSubDirectory);

                                MoveDirectory(path, newTrunkPath);
                            }
                            else
                            {
                                MoveDirectory(path, Path.Combine(Controls.WorkingDirectory, currentLine));
                            }
                        }
                        else
                        {
                            throw new Exception($"Path is not a file or directory: {path}");
                        }
                    }
                }

                DeleteDirectory(trunkPath);

                if (!string.IsNullOrEmpty(renamedTrunkSubDirectory))
                {
                    string renamedPath = Path.Combine(Controls.WorkingDirectory, renamedTrunkSubDirectory);
                    string originalPath = renamedPath.Substring(0, renamedPath.Length - 33);
                    Directory.Move(renamedPath, originalPath);
                }
            }
            finally
            {
                if (tempPath != null)
                {
                    File.Delete(tempPath);
                }
            }
        }

        internal static void MoveDirectory(string source, string destination)
        {
            int count = 0;
            while (true)
            {
                try
                {
                    Directory.Move(source, destination);
                    break;
                }
                catch
                {
                    if (++count <= 5)
                    {
                        int sleepTime = (Controls.SleepMilliseconds > 0 ? Controls.SleepMilliseconds : 100);
                        Thread.Sleep(sleepTime);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        private static void DeleteDirectory(string path)
        {
            int count = 0;
            while (true)
            {
                try
                {
                    Directory.Delete(path);
                    break;
                }
                catch
                {
                    if (++count <= 5)
                    {
                        int sleepTime = (Controls.SleepMilliseconds > 0 ? Controls.SleepMilliseconds : 100);
                        Thread.Sleep(sleepTime);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}
