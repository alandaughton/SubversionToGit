namespace SubversionToGit
{
    using System.IO;

    static class SetToRevision
    {
        public static void ExecuteWithDownload(int revision)
        {
            Logger.WriteInfo("START DOWNLOAD REVISION");

            var command = SubversionCommand.CreatePullFilesAtRevision(Controls.SubversionRepository, revision, Controls.WorkingDirectory);
            Shell.Execute(command);

            if (Controls.DiscardTrunk)
            {
                DiscardTrunk.Execute();
            }
        }

        public static void ExecuteWithPatch(int revision)
        {
            Logger.WriteInfo("START DOWNLOAD PATCH");

            if (Controls.DiscardTrunk)
            {
                RestoreTrunk.Execute();
            }

            string tempPath = null;
            try
            {
                tempPath = Path.GetTempFileName();
                var command = SubversionCommand.CreatePatch(Controls.SubversionRepository, revision, tempPath);
                Shell.Execute(command);

                command = SubversionCommand.ApplyPatch(tempPath, Controls.WorkingDirectory);
                Shell.Execute(command);
            }
            finally
            {
                if (tempPath != null)
                {
                    File.Delete(tempPath);
                }
            }

            if (Controls.DiscardTrunk)
            {
                DiscardTrunk.Execute();
            }
        }
    }
}
