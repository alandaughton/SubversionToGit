namespace SubversionToGit
{
    static class SubversionCommand
    {
        private const string AppName = "svn";

        public static ShellCommand CreatePatch(string pathToRepository, int revisionNumber, string outputPath)
        {
            string args = $"diff \"{pathToRepository}\" -r HEAD:{revisionNumber.ToString()} > \"{outputPath}\"";
            return new ShellCommand(AppName, args);
        }

        public static ShellCommand ApplyPatch(string patchPath, string workingFolder)
        {
            string args = $"patch -p0 -i \"{patchPath}\"";
            return new ShellCommand(AppName, args, workingFolder);
        }

        public static ShellCommand CreatePullFilesAtRevision(string pathToRepository, int revisionNumber, string workingFolder)
        {
            string args = $"export \"{pathToRepository}\" -r {revisionNumber.ToString()} --quiet --ignore-externals";
            return new ShellCommand(AppName, args, workingFolder);
        }

        public static ShellCommand CreateListRevisionsForTopLevelFolderXml(string pathToRepository, string outputPath)
        {
            string args = $"log \"{pathToRepository}\" --xml > \"{outputPath}\"";
            return new ShellCommand(AppName, args);
        }
/*
        public static ShellCommand CreateListXml(string pathToRepository, string outputPath)
        {
            string args = $"list \"{pathToRepository}\" --recursive --xml > \"{outputPath}\"";
            return new ShellCommand(AppName, args);
        }

        public static ShellCommand CreateFileRevisionHistoryXml(string pathToFile, string outputPath)
        {
            string args = $"log \"{pathToFile}\" --xml > \"{outputPath}\"";
            return new ShellCommand(AppName, args);
        }

        public static ShellCommand CreateChangesForRevisionXml(string pathToRepository, int revisionNumber, string outputPath)
        {
            string args = $"log \"{pathToRepository}\" -r {revisionNumber.ToString()} --verbose --xml > \"{outputPath}\"";
            return new ShellCommand(AppName, args);
        }

        public static ShellCommand CreateListFirstRevisionForPathXml(string pathToRepository, string outputPath)
        {
            string args = $"log \"{pathToRepository}\" -r 1:HEAD --limit 1 --xml > \"{outputPath}\"";
            return new ShellCommand(AppName, args);
        }
*/
    }
}
