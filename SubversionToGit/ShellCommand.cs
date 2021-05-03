namespace SubversionToGit
{
    sealed class ShellCommand
    {
        public ShellCommand(string appName, string args, string workingFolder = null)
        {
            this.AppName = appName;
            this.Args = args;
            this.WorkingFolder = workingFolder;
        }

        public string AppName { get; }

        public string Args { get; }

        public string WorkingFolder { get; }
    }
}
