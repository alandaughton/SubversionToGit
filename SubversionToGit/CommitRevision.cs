namespace SubversionToGit
{
    using System.IO;
    using System.Linq;

    static class CommitRevision
    {
        public static void Execute(int revision, string author, string date, string message)
        {
            Logger.WriteStartRevision($"START REVISION {revision.ToString()}");

            ExecuteFullFileDownload(revision);
            //ExecuteWithPatch(revision);

            bool needCommit = true;
            if (State.CurrentRevision == -1 && Controls.DiscardTrunk)
            {
                // First revision may merely create the trunk folder, which was discarded
                var dirInfo = new DirectoryInfo(Controls.WorkingDirectory);
                needCommit = dirInfo.EnumerateDirectories().Any(d => d.Name != ".git") || dirInfo.EnumerateFiles().Any();
            }

            if (needCommit)
            {
                GitCommit.Execute(author, date, message);
            }

            Logger.WriteInfo($"END REVISION {revision.ToString()}");
        }

        private static void ExecuteFullFileDownload(int revision)
        {
            EraseFiles.Execute();
            SetToRevision.ExecuteWithDownload(revision);
        }

        private static void ExecuteWithPatch(int revision)
        {
            SetToRevision.ExecuteWithPatch(revision);
        }
    }
}
