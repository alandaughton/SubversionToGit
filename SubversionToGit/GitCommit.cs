namespace SubversionToGit
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    static class GitCommit
    {
        private const string format_iso = "yyyy-MM-ddTHH:mm:ss";

        public static void Execute(string author, string date, string message)
        {
            Logger.WriteInfo("GIT COMMIT");

            var command = GitCommand.GitAddAll(Controls.WorkingDirectory);
            Shell.Execute(command);

            // The DateTime.Parse auto-converts to local time from UTC
            var checkinTimestamp = DateTime.Parse(date, CultureInfo.InvariantCulture, DateTimeStyles.None);

            string commitDateValue = checkinTimestamp.ToString(format_iso);
            string commitMessage = string.IsNullOrWhiteSpace(message) ? "NO CHECK-IN MESSAGE TO SVN" : message.Trim();

            string gitAuthor = Controls.GitAuthorFromSvnAuthor(author);

            int startEmailIndex = gitAuthor.IndexOf('<');
            string committerName = gitAuthor.Substring(0, startEmailIndex).Trim();
            string committerEmail = gitAuthor.Substring(startEmailIndex + 1).Replace(">", string.Empty).Trim();

            var environmentVariables = new Dictionary<string, string>();
            environmentVariables["GIT_AUTHOR_NAME"] = committerName;
            environmentVariables["GIT_AUTHOR_EMAIL"] = committerEmail;
            environmentVariables["GIT_AUTHOR_DATE"] = commitDateValue;
            environmentVariables["GIT_COMMITTER_NAME"] = committerName;
            environmentVariables["GIT_COMMITTER_EMAIL"] = committerEmail;
            environmentVariables["GIT_COMMITTER_DATE"] = commitDateValue;

            command = GitCommand.CommitWithMessage(Controls.WorkingDirectory, commitMessage);
            Shell.Execute(command, environmentVariables);
        }
    }
}
