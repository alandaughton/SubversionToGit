namespace SubversionToGit
{
    class ControlData
    {
        public string SubversionRepository { get; set; }
        public string WorkingDirectory { get; set; }
        public string AuthorsPath { get; set; }
        public string LogFilePath { get; set; }
        public int SleepMilliseconds { get; set; }
        public bool Quiet { get; set; }
        public bool Verbose { get; set; }
        public bool Debug { get; set; }
        public bool AbbreviatedLogging { get; set; }
    }
}
