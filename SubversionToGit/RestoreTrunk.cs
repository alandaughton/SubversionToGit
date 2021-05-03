namespace SubversionToGit
{
    using System;
    using System.IO;
    using System.Threading;

    static class RestoreTrunk
    {
        public static void Execute()
        {
            Logger.WriteInfo("Restoring files to be under trunk");

            string tempPath = null;
            try
            {
                string trunkPath = Path.Combine(Controls.WorkingDirectory, "trunk");

                string renamedTrunkSubDirectory = null;
                bool hasTrunkSubdirectory = Directory.Exists(trunkPath);
                if (hasTrunkSubdirectory)
                {
                    var info = new DirectoryInfo(trunkPath);

                    var trunkSuffix = Guid.NewGuid().ToString("D");
                    renamedTrunkSubDirectory = $"{info.FullName}_{trunkSuffix}"; // adds 33 characters to the path
                    DiscardTrunk.MoveDirectory(info.FullName, renamedTrunkSubDirectory);
                }

                tempPath = Path.GetTempFileName();
                var command = DirectoryCommand.ListFilesAndFolders(Controls.WorkingDirectory, tempPath);
                Shell.Execute(command);

                Directory.CreateDirectory(trunkPath);

                using (var reader = File.OpenText(tempPath))
                {
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        string currentLine = line;
                        line = reader.ReadLine();

                        if (currentLine == ".git")
                        {
                            continue;
                        }

                        string path = Path.Combine(Controls.WorkingDirectory, currentLine);
                        if (File.Exists(path))
                        {
                            File.Move(path, Path.Combine(trunkPath, currentLine));
                        }
                        else if (Directory.Exists(path))
                        {
                            DiscardTrunk.MoveDirectory(path, Path.Combine(trunkPath, currentLine));
                        }
                        else
                        {
                            throw new Exception($"Path is not a file or directory: {path}");
                        }
                    }
                }

                if (hasTrunkSubdirectory)
                {
                    string directoryName = Path.GetDirectoryName(renamedTrunkSubDirectory);
                    directoryName = directoryName.Substring(0, directoryName.Length - 33);
                    DiscardTrunk.MoveDirectory(renamedTrunkSubDirectory, Path.Combine(trunkPath, directoryName));
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
    }
}
