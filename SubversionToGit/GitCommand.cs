namespace SubversionToGit
{
    using System.Collections.Generic;
    using System.IO;

    static class GitCommand
    {
        private const string AppName = "git";

        public static ShellCommand InitializeGit(string workingDirectory)
        {
            return new ShellCommand(AppName, "init --quiet", workingDirectory);
        }

        public static ShellCommand GitAddAll(string workingDirectory)
        {
            return new ShellCommand(AppName, "add --all", workingDirectory);
        }

        public static ShellCommand CommitWithMessage(string workingDirectory, string commitMessage)
        {
            bool useFile = commitMessage.Contains("\"") || commitMessage.Contains("$");
            if (!useFile)
            {
                using (var reader = new StringReader(commitMessage))
                {
                    string line = reader.ReadLine();
                    line = reader.ReadLine();
                    useFile = line != null;
                }
            }

            if (!useFile)
            {
                string args = $"commit --message=\"{commitMessage}\"";
                return new ShellCommand(AppName, args, workingDirectory);
            }
            else
            {
                string path = Controls.GetCommitMessageFilePath();
                File.WriteAllText(path, commitMessage);

                string args = $"commit --file=\"{path}\"";
                return new ShellCommand(AppName, args, workingDirectory);
            }
        }
    }
}
