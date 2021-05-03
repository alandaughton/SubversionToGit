namespace SubversionToGit
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    static class EraseFiles
    {
        public static void Execute()
        {
            Logger.WriteInfo("Erasing current files");

            string tempPath = null;
            try
            {
                tempPath = Path.GetTempFileName();
                var command = DirectoryCommand.ListFilesAndFolders(Controls.WorkingDirectory, tempPath);
                Shell.Execute(command);

                using (var reader = File.OpenText(tempPath))
                {
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        string currentLine = line;
                        line = reader.ReadLine();

                        if (currentLine != ".git")
                        {
                            string path = Path.Combine(Controls.WorkingDirectory, currentLine);
                            if (File.Exists(path))
                            {
                                File.Delete(path);
                            }
                            else if (Directory.Exists(path))
                            {
                                Directory.Delete(path, recursive: true);
                            }
                            else
                            {
                                throw new Exception($"Path is not a file or directory: {path}");
                            }
                        }
                    }
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
