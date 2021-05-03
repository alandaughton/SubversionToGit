namespace SubversionToGit
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    static class Controls
    {
        private static ControlData theData;

        public static void SetData(ControlData data)
        {
            theData = data;
            ReadAuthors();
        }

        public static string SubversionRepository => theData.SubversionRepository;

        public static string WorkingDirectory => theData.WorkingDirectory;

        public static string LogFilePath => theData.LogFilePath;

        public static int SleepMilliseconds => theData.SleepMilliseconds;

        public static bool Quiet => theData.Quiet;

        public static bool Verbose => theData.Verbose;

        public static bool Debug => theData.Debug;

        public static bool AbbreviatedLogging => theData.AbbreviatedLogging;

        public static bool DiscardTrunk => theData.SubversionRepository.EndsWith("/trunk", StringComparison.OrdinalIgnoreCase);

        private static Dictionary<string, string> AuthorsMap { get; set; }

        public static string GitAuthorFromSvnAuthor(string svnAuthor)
        {
            if (!AuthorsMap.ContainsKey(svnAuthor))
            {
                throw new Exception($"Missing SVN author {svnAuthor}");
            }
            else
            {
                return AuthorsMap[svnAuthor];
            }
        }

        public static string GetCommitMessageFilePath()
        {
            string directory = Path.GetDirectoryName(Controls.LogFilePath);
            return Path.Combine(directory, "SubversionToGit_CommitMessage.txt");
        }

        private static void ReadAuthors()
        {
            AuthorsMap = new Dictionary<string, string>();
            using (var reader = File.OpenText(theData.AuthorsPath))
            {
                string line = reader.ReadLine();
                while (line != null)
                {
                    string currentLine = line;
                    line = reader.ReadLine();

                    // Allow adding comments - if for no other reason than to test when authors not found
                    if (currentLine.StartsWith("//"))
                    {
                        continue;
                    }

                    var pieces = currentLine.Split('=');
                    string svnAuthor = pieces[0].Trim();
                    string gitAuthor = pieces[1].Trim();

                    if (string.IsNullOrWhiteSpace(svnAuthor) || string.IsNullOrWhiteSpace(gitAuthor))
                    {
                        if (!string.IsNullOrWhiteSpace(svnAuthor))
                        {
                            throw new Exception($"Missing GIT author for SVN author {svnAuthor}");
                        }
                        else if (!string.IsNullOrWhiteSpace(gitAuthor))
                        {
                            throw new Exception($"Missing SVN author for GIT author {gitAuthor}");
                        }
                        else
                        {
                            throw new Exception("Blank entry in the authors file");
                        }
                    }

                    if (AuthorsMap.ContainsKey(svnAuthor))
                    {
                        throw new Exception($"Duplicate author entry : {svnAuthor}");
                    }

                    AuthorsMap[svnAuthor] = gitAuthor;
                }
            }
        }
    }
}
