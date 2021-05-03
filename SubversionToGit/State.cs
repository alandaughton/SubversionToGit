namespace SubversionToGit
{
    using System.IO;

    static class State
    {
        public static int CurrentRevision = -1;

        public static void Load()
        {
            using (var reader = File.OpenText(Controls.LogFilePath))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line.StartsWith("END REVISION "))
                    {
                        int endIndex = line.IndexOf('[');
                        string revision = line.Substring(13, endIndex - 13).Trim();
                        CurrentRevision = int.Parse(revision);
                    }

                    line = reader.ReadLine();
                }
            }
        }
    }
}
