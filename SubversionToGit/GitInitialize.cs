namespace SubversionToGit
{
    static class GitInitialize
    {
        public static void Execute()
        {
            Logger.WriteInfo("START GIT INIT");
            var command = GitCommand.InitializeGit(Controls.WorkingDirectory);
            Shell.Execute(command);
        }
    }
}
